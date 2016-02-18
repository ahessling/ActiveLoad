/**
 * @file HMI_USB.hpp
 *
 * @date 09.12.2015
 * @author Andre
 * @description
 */
#ifndef HMI_USB_HPP_
#define HMI_USB_HPP_

#include <stdint.h>
#include "HMI.hpp"
#include "SCPICommand.hpp"
#include "NVRAM.hpp"

#define HMI_USB_RX_LENGTH       128 // 2^N
#define HMI_USB_CMD_LENGTH      127

class HMI_USB : HMI
{
public:
  HMI_USB(NVRAM* nvRAM);
  virtual ~HMI_USB() { }

  virtual bool execute(SystemState& systemState, SystemCommand& systemCommand);

  static uint16_t vcp_init(void);
  static uint16_t vcp_deinit(void);
  static uint16_t vcp_ctrl(uint32_t cmd, uint8_t* buf, uint32_t len);
  static uint16_t vcp_data_send_cb(uint8_t* buf, uint32_t len);
  static uint16_t vcp_data_receive_cb(uint8_t* buf, uint32_t len);

private:
  enum CommandResponse
  {
    CR_INVALID_PARAM = -2,
    CR_UNKNOWN = -1,
    CR_OK = 0,
    CR_NONE
  };

  int readChar();
  SCPICommand _scpiCommandParser;
  CommandResponse scpiStringToCommand(SystemCommand& systemCommand, SystemState& systemState);
  CommandResponse respondToSCPIQuery(SystemState& systemState);

  static HMI_USB* _this;

  char _rxBuf[HMI_USB_RX_LENGTH];
  unsigned int _rxBufHead;
  unsigned int _rxBufTail;
  unsigned int _rxBufLength;

  char _cmdLine[HMI_USB_CMD_LENGTH + 1];
  unsigned int _cmdLinePos;

  unsigned int _calibStep[2];
  float _calibActualVoltageX[2], _calibActualVoltageY[2];
  float _calibActualCurrentX[2], _calibActualCurrentY[2];
  float _calibSetpointCurrentX[2], _calibSetpointCurrentY[2];

  NVRAM* _nvRAM;
};

// syscalls for printf and friends
extern "C"
{
  int _read(int file, char *ptr, int len);

  int _write(int file, char *ptr, int len);
}



#endif /* HMI_USB_HPP_ */
