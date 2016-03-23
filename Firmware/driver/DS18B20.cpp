/**
 * @file DS18B20.cpp
 *
 * @date 14.12.2015
 * @author Andre
 * @description
 */

#include "DS18B20.hpp"

#define TEMP_CONVERSION_TIMEOUT     1000 // ms

/** DS18B20 Dallas 1-wire device.
 *
 * @param dataGPIO Data port
 * @param dataPin Data pin
 */
DS18B20::DS18B20(GPIO_TypeDef* dataGPIO, uint16_t dataPin) : Dallas1Wire(dataGPIO, dataPin)
{
  _scratchpadValid = false;
  _conversionStarted = false;
  _temperatureSensor = TemperatureSensor::Unknown;
}

/** Reads the DS18B20 temperature from scratchpad.
 *
 * A conversion must have been triggered before using startConversion.
 * @param temperature Pointer to a float variable which will hold the temperature
 * @return 0 if temperature is valid; -1 if device not ready;
 *         -2 in case of CRC error
 */
int DS18B20::readTemperature(float* temperature)
{
  // make sure that a temperature conversion has been started before
  if (false == _conversionStarted)
  {
    return -1;
  }

  int result = readScratchpad();

  if (0 == result)
  {
    *temperature = ((int16_t)_scratchpad.tempLow) | ((int16_t)_scratchpad.tempHigh << 8);

    if (TemperatureSensor::DS18B20Type == _temperatureSensor)
    {
      // DS18S20 type has 12 bit resolution (0.0625 Kelvin per LSB)
      *temperature *= 0.0625;
    }
    else if (TemperatureSensor::DS18S20Type == _temperatureSensor)
    {
      // DS18S20 type has 9 bit resolution (0.5 Kelvin per LSB)
      *temperature *= 0.5;
    }
    else
    {
      // unknown type
      result = -1;
    }
  }

  return result;
}

/** Starts a temperature conversion.
 *
 * @param wait True if function should block until temperature is converted
 * @return 0 if temperature has been converted; -1 if otherwise
 */
int DS18B20::startConversion(bool wait)
{
  int result = 0;

  if (TemperatureSensor::Unknown == _temperatureSensor)
  {
    // no conversion started before (first start)

    // identify chip (DS18B20 or DS18S20)
    result = readROM();

    if (0 != result)
    {
      // could not identify chip
      return result;
    }
    else
    {
      if (_romCode[0] == 0x28)
      {
        _temperatureSensor = TemperatureSensor::DS18B20Type;
      }
      else if (_romCode[0] == 0x10)
      {
        _temperatureSensor = TemperatureSensor::DS18S20Type;
      }
    }
  }

  result = reset();

  if (0 == result)
  {
    writeByte(0xCC); // Skip ROM
    writeByte(0x44); // Convert T

    if (true == wait)
    {
      // wait until conversion is completed
      result = busyWait();
    }

    _conversionStarted = true;
  }

  return result;
}

/** Reads the DS18B20 scratchpad.
 *
 * @return 0 if scratchpad is valid; -1 if device not ready;
 *         -2 if scratchpad invalid (CRC error)
 */
int DS18B20::readScratchpad()
{
  int result = reset();

  if (0 == result)
  {
    writeByte(0xCC); // Skip ROM
    writeByte(0xBE); // Read Scratchpad

    for (unsigned int i = 0; i < 9; i++)
    {
      *((uint8_t*)&_scratchpad + i) = readByte();
    }

    // check CRC
    if (Dallas1Wire::calcCRC((char*)&_scratchpad, sizeof(_scratchpad)) == 0)
    {
      _scratchpadValid = true;
    }
    else
    {
      _scratchpadValid = false;
    }

    result = (_scratchpadValid == true) ? 0 : -2;
  }

  return result;
}

/** DS18B20 busy waiting.
 *
 * @return 0 if no timeout has occurred.
 */
int DS18B20::busyWait()
{
  unsigned int enterTime = mstimer_get();

  while ((!readBit()) && (mstimer_get() - enterTime < TEMP_CONVERSION_TIMEOUT));

  if ((mstimer_get() - enterTime) >= TEMP_CONVERSION_TIMEOUT)
  {
    return -1;
  }
  else
  {
    return 0;
  }
}

/** Read 64 bit unique ROM code.
 *
 * @return 0 if no timeout has occured.
 */
int DS18B20::readROM()
{
  int result = reset();

  if (0 == result)
  {
    writeByte(0x33); // Read ROM

    // read 64 bit unique code
    for (unsigned int i = 0; i < 8; i++)
    {
      _romCode[i] = readByte();
    }
  }

  return result;
}
