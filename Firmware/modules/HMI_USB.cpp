/**
 * @file HMI_USB.cpp
 *
 * @date 09.12.2015
 * @author Andre
 * @description
 */
#include "HMI_USB.hpp"
#include "usbd_cdc_core.h"
#include "usbd_usr.h"
#include <stdio.h>
#include "core_cmFunc.h"
#include <string.h>
#include "version.h"
#include "hw_config.h"
#include "tools/systimer.h"

#define IDN_STRING      "Andre Hessling,Active Load V1,0," VERSION_STRING

HMI_USB* HMI_USB::_this;

// callback functions from USB stack

CDC_IF_Prop_TypeDef VCP_fops =
{
  HMI_USB::vcp_init,
  HMI_USB::vcp_deinit,
  HMI_USB::vcp_ctrl,
  HMI_USB::vcp_data_send_cb,
  HMI_USB::vcp_data_receive_cb
};

enum Subsystem
{
  SUBSYSTEM_NONE,
  SUBSYSTEM_SOURCE,
  SUBSYSTEM_MEASURE,
  SUBSYSTEM_CALIBRATE,
};

HMI_USB::HMI_USB(NVRAM* nvRAM) : HMI()
{
  _this = this;

  _nvRAM = nvRAM;

  // init RX buffer
  _rxBufHead = 0;
  _rxBufTail = 0;
  _rxBufLength = 0;

  // init command line
  _cmdLinePos = 0;

  // init USB stack
  USBD_Init(&USB_Device_dev,
                &usb_usr_desc,
                &usbd_cdc_cb,
                &usb_usr_cb);

  // reset calibration steps
  _calibStep[0] = 0;
  _calibStep[1] = 0;
}

bool HMI_USB::execute(SystemState& systemState, SystemCommand& systemCommand)
{
  int ch;
  bool commandAvailable = false;

  // read all characters in USB RX buffer
  while ((ch = readChar()) != -1)
  {
    if (_cmdLinePos < HMI_USB_CMD_LENGTH)
    {
      switch (ch)
      {
      case '\r':
      case '\n':
        // terminate command string
        _cmdLine[_cmdLinePos] = '\0';
        commandAvailable = true;
        break;

      default:
        // copy into command line buffer
        _cmdLine[_cmdLinePos++] = ch & 0xFF;
      }

      if (true == commandAvailable)
      {
        // tell SCPI parser that new data arrived
        _scpiCommandParser.parseSCPICommand(_cmdLine, sizeof(_cmdLine));

        // check if this is a valid query
        if (_scpiCommandParser.isQuery() == true)
        {
          HMI_USB::CommandResponse commandResponse = respondToSCPIQuery(systemState, systemCommand);

          if (CR_UNKNOWN == commandResponse)
          {
            printf("ERR: Unknown query\n");
          }

          commandAvailable = false;
        }
        else
        {
          // parse SCPI string and produce a SystemCommand, if
          // a valid command has been detected
          HMI_USB::CommandResponse commandResponse = scpiStringToCommand(systemCommand, systemState);
          commandAvailable = commandResponse != CR_UNKNOWN ? true : false;

          if (CR_OK == commandResponse)
          {
            printf("OK\n");
          }
          else if (CR_UNKNOWN == commandResponse)
          {
            printf("ERR: Unknown command\n");
          }
          else if (CR_INVALID_PARAM == commandResponse)
          {
            printf("ERR: Invalid command parameters\n");
          }
        }

        // reset command line
        _cmdLinePos = 0;

        break;
      }
    }
    else
    {
      // no space in command buffer, reset
      _cmdLinePos = 0;
      printf("ERROR: TOO LONG\n");
    }
  }

  return commandAvailable;
}

HMI_USB::CommandResponse HMI_USB::respondToSCPIQuery(SystemState& systemState, const SystemCommand& systemCommand)
{
  // process all tokens
  char **scpiTokens = _scpiCommandParser.getSCPITokens();
  unsigned int hierarchy = 0;

  enum Subsystem subsystem = SUBSYSTEM_NONE;

  for (int tokenIndex = 0; tokenIndex < _scpiCommandParser.getSCPITokenCount(); tokenIndex++)
  {
    bool query;
    char parsedCommand[32];
    SCPICommand::getSCPICommand(scpiTokens[tokenIndex], parsedCommand, sizeof(parsedCommand), query);

    if (!strcmp(parsedCommand, ""))
    {
      // skip empty tokens
      continue;
    }

    if (0 == hierarchy)
    {
      if (!strcmp(parsedCommand, "SOUR"))
      {
        subsystem = SUBSYSTEM_SOURCE;
      }
      else if (!strcmp(parsedCommand, "CURR"))
      {
        printf("%d mA\n", (int)(systemState.setpointCurrent * 1000));
        return CR_OK;
      }
      else if (!strcmp(parsedCommand, "MEAS"))
      {
        subsystem = SUBSYSTEM_MEASURE;
      }
      else if (!strcmp(parsedCommand, "*IDN"))
      {
        printf("%s\n", IDN_STRING);
        return CR_OK;
      }
      else if (!strcmp(parsedCommand, "UPTI"))
      {
        printf("%lu seconds\n", (uint32_t)(get_uptime_ms() / 1000));
        return CR_OK;
      }
      else if (!strcmp(parsedCommand, "DUMP"))
      {
        dumpSystemState(systemState, systemCommand);
        return CR_OK;
      }
    }
    else if (SUBSYSTEM_SOURCE == subsystem)
    {
      if (!strcmp(parsedCommand, "CURR"))
      {
        printf("%d mA\n", (int)(systemState.setpointCurrent * 1000));
        return CR_OK;
      }
    }
    else if (SUBSYSTEM_MEASURE == subsystem)
    {
      if (!strcmp(parsedCommand, "CURR"))
      {
        printf("%d mA\n", (int)(systemState.actualCurrent * 1000));
        return CR_OK;
      }
      else if (!strcmp(parsedCommand, "VOLT"))
      {
        printf("%d mV\n", (int)(systemState.actualVoltage * 1000));
        return CR_OK;
      }
      else if (!strcmp(parsedCommand, "TEMP"))
      {
        printf("%d C\n", (int)(systemState.temperaturePower + 0.5));
        return CR_OK;
      }
    }

    hierarchy++;
  }

  return CR_UNKNOWN;
}

HMI_USB::CommandResponse HMI_USB::scpiStringToCommand(SystemCommand& systemCommand,
    SystemState& systemState)
{
  // process all tokens
  char **scpiTokens = _scpiCommandParser.getSCPITokens();
  unsigned int hierarchy = 0;

  enum Subsystem subsystem = SUBSYSTEM_NONE;

  for (int tokenIndex = 0; tokenIndex < _scpiCommandParser.getSCPITokenCount(); tokenIndex++)
  {
    bool query;
    char parsedCommand[32];

    SCPICommand::getSCPICommand(scpiTokens[tokenIndex], parsedCommand, sizeof(parsedCommand), query);

    if (!strcmp(parsedCommand, ""))
    {
      // skip empty tokens
      continue;
    }

    const char *param = _scpiCommandParser.getSCPIParam();

    if (0 == hierarchy)
    {
      if (!strcmp(parsedCommand, "SOUR"))
      {
        subsystem = SUBSYSTEM_SOURCE;
      }
      else if (!strcmp(parsedCommand, "CURR"))
      {
        int value;
        if (sscanf(param, "%d", &value) == 1)
        {
          systemCommand.setpointCurrent = value / 1000.0; // mA --> A
          return CR_OK;
        }
      }
      else if (!strcmp(parsedCommand, "CALIB"))
      {
        subsystem = SUBSYSTEM_CALIBRATE;
      }
      else if (!strcmp(parsedCommand, "*RST"))
      {
        return CR_OK;
      }
      else if (!strcmp(parsedCommand, "*DFU"))
      {
        // jump to embedded bootloader code
        jump_to(0x1FFFC800);
        return CR_OK;
      }
    }
    else if (SUBSYSTEM_SOURCE == subsystem)
    {
      if (!strcmp(parsedCommand, "CURR"))
      {
        int value;
        if (sscanf(param, "%d", &value) == 1)
        {
          systemCommand.setpointCurrent = value / 1000.0; // mA --> A
          return CR_OK;
        }
      }
    }
    else if (SUBSYSTEM_CALIBRATE == subsystem)
    {
      if (!strcmp(parsedCommand, "CLEAR"))
      {
        // clear all calibration data
        systemCommand.calibSetpointCurrent.clearCalibrationData();
        systemState.calibActualVoltage.clearCalibrationData();
        systemState.calibActualCurrent.clearCalibrationData();

        // save cleared calibration data to NVRAM
        _nvRAM->saveMemorySlot(NVRAM::MemoryLayout::CalibSetpointCurrent, systemCommand.calibSetpointCurrent);
        _nvRAM->saveMemorySlot(NVRAM::MemoryLayout::CalibActualVoltage, systemState.calibActualVoltage);
        _nvRAM->saveMemorySlot(NVRAM::MemoryLayout::CalibActualCurrent, systemState.calibActualCurrent);

        return CR_OK;
      }
      else if (!strcmp(parsedCommand, "CURR"))
      {
        int value;

        // calibration of setpoint and actual current
        switch (_calibStep[0])
        {
        case 0:
          printf("WARN: Entering calibration mode. Connect a voltage source capable of delivering up to 1A! Type this command again and follow instructions.\n");
          _calibStep[0]++;

          return CR_NONE;

        case 1:
          // clear calibration data first
          systemCommand.calibSetpointCurrent.clearCalibrationData();
          systemState.calibActualCurrent.clearCalibrationData();

          printf("Measure load current with a calibrated device and type CALIB:CURR <Measured current [mA]>\n");

          // first setpoint
          systemCommand.setpointCurrent = 0.1f; // 100 mA
          _calibSetpointCurrentX[0] = systemCommand.setpointCurrent;

          _calibStep[0]++;
          return CR_NONE;

        case 2:
          if (sscanf(param, "%d", &value) == 1)
          {
            // save first measurement
            _calibSetpointCurrentY[0] = value / 1000.0; // mA --> A

            // measured current
            _calibActualCurrentX[0] = _calibSetpointCurrentY[0];
            _calibActualCurrentY[0] = systemState.actualCurrent;

            printf("Measure load current with a calibrated device and type CALIB:CURR <Measured current [mA]>\n");

            // second setpoint
            systemCommand.setpointCurrent = 1.0f; // 1 A
            _calibSetpointCurrentX[1] = systemCommand.setpointCurrent;

            _calibStep[0]++;
            return CR_NONE;
          }
          else
          {
            return CR_UNKNOWN;
          }

        case 3:
          if (sscanf(param, "%d", &value) == 1)
          {
            // save second setpoint
            _calibSetpointCurrentY[1] = value / 1000.0; // mA --> A

            // measured current
            _calibActualCurrentX[1] = _calibSetpointCurrentY[1];
            _calibActualCurrentY[1] = systemState.actualCurrent;

            // calculate calibration data
            systemCommand.calibSetpointCurrent.calibrate(_calibSetpointCurrentX, _calibSetpointCurrentY);
            systemState.calibActualCurrent.calibrate(_calibActualCurrentX, _calibActualCurrentY);

            // save calibration data to NVRAM
            _nvRAM->saveMemorySlot(NVRAM::MemoryLayout::CalibSetpointCurrent, systemCommand.calibSetpointCurrent);
            _nvRAM->saveMemorySlot(NVRAM::MemoryLayout::CalibActualCurrent, systemState.calibActualCurrent);

            printf("Calibration done.\n");

            systemCommand.setpointCurrent = 0.0f;

            _calibStep[0] = 0;
            return CR_NONE;
          }
          else
          {
            return CR_UNKNOWN;
          }
        }

      }
      else if (!strcmp(parsedCommand, "VOLT"))
      {
        int value;

        // calibration of actual voltage
        switch (_calibStep[1])
        {
        case 0:
          printf("WARN: Entering calibration mode. Type this command again and follow instructions.\n");
          _calibStep[1]++;

          return CR_NONE;

        case 1:
          // clear calibration data first
          systemState.calibActualVoltage.clearCalibrationData();

          // disconnect load, so no current flow can disturb this measurement
          systemCommand.setpointCurrent = 0;

          printf("Set voltage source to 5-10V and type CALIB:VOLT <Measured voltage [mV]>\n");

          _calibStep[1]++;

          return CR_NONE;

        case 2:
          if (sscanf(param, "%d", &value) == 1)
          {
            // measured voltage (internally)
            _calibActualVoltageX[0] = value / 1000.0; // mV --> V
            _calibActualVoltageY[0] = systemState.actualVoltage;

            printf("Set voltage source to 20-30V and type CALIB:VOLT <Measured voltage [mV]>\n");

            _calibStep[1]++;

            return CR_NONE;
          }
          else
          {
            return CR_UNKNOWN;
          }

        case 3:
          if (sscanf(param, "%d", &value) == 1)
          {
            // measured voltage (internally)
            _calibActualVoltageX[1] = value / 1000.0; // mV --> V
            _calibActualVoltageY[1] = systemState.actualVoltage;

            systemState.calibActualVoltage.calibrate(_calibActualVoltageX, _calibActualVoltageY);

            // save calibration data to NVRAM
            _nvRAM->saveMemorySlot(NVRAM::MemoryLayout::CalibActualVoltage, systemState.calibActualVoltage);

            printf("Calibration done.\n");

            _calibStep[1] = 0;

            return CR_NONE;
          }
          else
          {
            return CR_UNKNOWN;
          }
        }
      }
    }

    hierarchy++;
  }

  return CR_UNKNOWN;
}

void HMI_USB::dumpSystemState(const SystemState& systemState, const SystemCommand& systemCommand)
{
  printf("Setpoint current      : %.3fA\n", systemState.setpointCurrent);
  printf("Actual voltage        : %.3fV\n", systemState.actualVoltage);
  printf("Actual current        : %.3fA\n", systemState.actualCurrent);
  printf("Temperature power     : %.1f C\n", systemState.temperaturePower);
  printf("Calib actual voltage  : %s\n", systemState.calibActualVoltage.isCalibrated() ? "YES":"NO");
  printf("Calib actual current  : %s\n", systemState.calibActualCurrent.isCalibrated() ? "YES":"NO");
  printf("Calib setpoint current: %s\n", systemCommand.calibSetpointCurrent.isCalibrated() ? "YES":"NO");
}

int HMI_USB::readChar()
{
  int ch = -1;

  __disable_irq();

  if (_rxBufLength > 0)
  {
    // consume one character
    ch = _rxBuf[_rxBufHead++];
    _rxBufHead &= (HMI_USB_RX_LENGTH - 1);
    _rxBufLength--;
  }

  __enable_irq();

  return ch;
}

/**
  * @brief  VCP_Init
  *         Initializes the Media on the STM32
  * @param  None
  * @retval Result of the operation (USBD_OK in all cases)
  */
uint16_t HMI_USB::vcp_init(void)
{
  return USBD_OK;
}

/**
  * @brief  VCP_DeInit
  *         DeInitializes the Media on the STM32
  * @param  None
  * @retval Result of the opeartion (USBD_OK in all cases)
  */
uint16_t HMI_USB::vcp_deinit(void)
{

  return USBD_OK;
}


/**
  * @brief  VCP_Ctrl
  *         Manage the CDC class requests
  * @param  cmd: Command code
  * @param  buf: Buffer containing command data (request parameters)
  * @param  len: Number of data to be sent (in bytes)
  * @retval Result of the operation (USBD_OK in all cases)
  */
uint16_t HMI_USB::vcp_ctrl (uint32_t cmd, uint8_t* buf, uint32_t len)
{
  (void)cmd;
  (void)buf;
  (void)len;

  return USBD_OK;
}

/**
  * @brief  VCP_DataTx
  *         CDC received data to be send over USB IN endpoint are managed in
  *         this function.
  * @param  buf: Buffer of data to be sent
  * @param  len: Number of data to be sent (in bytes)
  * @retval Result of the operation: USBD_OK if all operations are OK else VCP_FAIL
  */
uint16_t HMI_USB::vcp_data_send_cb (uint8_t* buf, uint32_t len)
{
  (void)buf;
  (void)len;

  return USBD_OK;
}

/**
  * @brief  VCP_DataRx
  *         Data received over USB OUT endpoint are sent over CDC interface
  *         through this function.
  * @note   This function will block any OUT packet reception on USB endpoint
  *         untill exiting this function. If you exit this function before transfer
  *         is complete on CDC interface (ie. using DMA controller) it will result
  *         in receiving more data while previous ones are still not sent.
  *
  * @param  buf: Buffer of data to be received
  * @param  len: Number of data received (in bytes)
  * @retval Result of the operation: USBD_OK if all operations are OK else VCP_FAIL
  */
uint16_t HMI_USB::vcp_data_receive_cb (uint8_t* buf, uint32_t len)
{
  while (len--)
  {
    // check if space is available in buffer
    if (_this->_rxBufLength < HMI_USB_RX_LENGTH)
    {
      // add character to buffer
      _this->_rxBuf[_this->_rxBufTail++] = *buf++;
      _this->_rxBufTail &= (HMI_USB_RX_LENGTH - 1);
      _this->_rxBufLength++;
    }
    else
    {
      // buffer full, discard data
      break;
    }
  }

  return USBD_OK;
}

// syscalls for printf and friends

extern uint8_t  usb_app_rx_buffer []; /* Write CDC received data in this buffer.
                                     These data will be sent over USB IN endpoint
                                     in the CDC core functions. */
extern uint32_t usb_app_rx_ptr_in;    /* Increment this pointer or roll it back to
                                     start address when writing received data
                                     in the buffer APP_Rx_Buffer. */

int _read(int file, char *ptr, int len)
{
  (void)file;
  (void)ptr;
  (void)len;

  return 0;
}

int _write(int file, char *ptr, int len)
{
  (void)file;

  for (int i = 0; i < len; i++)
  {
    __disable_irq();

    usb_app_rx_buffer[usb_app_rx_ptr_in++] = *ptr++;

    // wrap around
    if (usb_app_rx_ptr_in >= USB_APP_RX_DATA_SIZE)
    {
      usb_app_rx_ptr_in = 0;
    }

    __enable_irq();
  }

  return len;
}

