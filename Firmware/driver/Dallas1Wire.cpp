/**
 * @file Dallas1Wire.cpp
 *
 * @date 14.12.2015
 * @author Andre
 * @description
 */

#include "Dallas1Wire.hpp"

/** Dallas 1-wire base device.
 *
 * @param dataGPIO Data port
 * @param dataPin Data pin
 */
Dallas1Wire::Dallas1Wire(GPIO_TypeDef* dataGPIO, uint16_t dataPin)
{
  _dataGPIO = dataGPIO;
  _dataPin = dataPin;

  // get pin position (0..15)
  _pinPos = getPinPos(_dataPin);

  // configure GPIO output driver
  _dataGPIO->OTYPER |= 1 << _pinPos; // output open-drain
  _dataGPIO->OSPEEDR |= GPIO_Speed_Level_3 << (2 * _pinPos); // high speed
}

/** Resets the 1-wire bus and determines if a device is found.
 *
 * @return 0 if a device is found; -1 if no device is found
 */
int Dallas1Wire::reset()
{
  setBusState(false);
  delay_us(480);
  setBusState(true);
  delay_us(70);

  if (getBusState() == false)
  {
    delay_us(410);
    return 0;
  }
  else
  {
    return -1;
  }
}

/** Write one byte via Dallas 1-wire.
 *
 * @param byte Byte to be transferred
 */
void Dallas1Wire::writeByte(char byte)
{
  for (unsigned int i = 0; i < 8; i++)
  {
    writeBit(byte & 0x01);
    byte >>= 1;
  }
}

/** Read one byte via Dallas 1-wire.
 *
 * @return Byte transferred
 */
char Dallas1Wire::readByte()
{
  char data = 0;

  for (unsigned int i = 0; i < 8; i++)
  {
    data >>= 1;

    if (readBit() == true)
    {
      data |= 0x80;
    }
  }

  return data;
}

/** Set 1-wire bus to input or output.
 *
 * @param output True for output; false for input
 */
void Dallas1Wire::setBusDirection(bool output)
{
  if (true == output)
  {
    _dataGPIO->MODER &= ~(GPIO_MODER_MODER0 << (2 * _pinPos));
    _dataGPIO->MODER |= 0x01 << (2 * _pinPos); // output mode
  }
  else
  {
    _dataGPIO->MODER &= ~(GPIO_MODER_MODER0 << (2 * _pinPos)); // input mode
  }
}

/** Put out a physical 1 or 0 via 1-wire.
 *
 * @param high True = 1, False = 0
 */
void Dallas1Wire::setBusState(bool high)
{
  if (true == high)
  {
    setBusDirection(false);
    _dataGPIO->BRR = _dataPin;
  }
  else
  {
    setBusDirection(true);
    _dataGPIO->BRR = _dataPin;
  }
}

/** Write a logical bit via 1-wire.
 *
 * @param bit True = 1, False = 0
 */
void Dallas1Wire::writeBit(bool bit)
{
  setBusState(false);

  if (true == bit)
  {
    delay_us(6);
    setBusState(true);
    delay_us(58);
  }
  else
  {
    delay_us(60);
    setBusState(true);
    delay_us(10);
  }
}

/** Reads the physical state of the 1-wire bus.
 *
 * @return True = 1, False = 0
 */
inline bool Dallas1Wire::getBusState()
{
  return _dataGPIO->IDR & _dataPin;
}

/** Read a logical bit via 1-wire.
 *
 * @return True = 1, False = 0
 */
bool Dallas1Wire::readBit()
{
  bool bit = false;

  setBusState(false);
  delay_us(6);
  setBusState(true);
  delay_us(9);

  if (getBusState() == true)
  {
    bit = 1;
  }

  delay_us(55);

  return bit;
}

/** Helper function for conversion of pin (0..0x8000) to pin position (0..15).
 *
 * @param pin Pin (0..0x8000)
 * @return Pin position (0..15)
 */
unsigned int Dallas1Wire::getPinPos(uint16_t pin)
{
  for (unsigned int pinpos = 0x00; pinpos < 0x10; pinpos++)
  {
    unsigned int pos = ((uint32_t)0x01) << pinpos;

    if (pin & pos)
    {
      // position found
      return pinpos;
    }
  }

  return 0;
}

#define CRC8POLY    0x18              //0X18 = X^8+X^5+X^4+X^0

/** Calculate Dallas 1-Wire CRC8.
 *
 * @param data Pointer to buffer with data
 * @param size Number of bytes in buffer
 * @return 8-bit Dallas CRC
 */
char Dallas1Wire::calcCRC(const char* data, unsigned int size)
{
  uint8_t crc = 0;

  while (size-- > 0)
  {
    uint8_t b = *data++;

    for (int bits = 0; bits < 8; bits++)
    {
      uint8_t  feedback_bit = (crc ^ b) & 0x01;

      if (feedback_bit == 0x01)
      {
        crc = crc ^ CRC8POLY;
      }

      crc = (crc >> 1) & 0x7F;

      if (feedback_bit == 0x01)
      {
        crc = crc | 0x80;
      }

      b = b >> 1;
    }
  }

  return crc;
}
