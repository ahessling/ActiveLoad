/**
 * @file Dallas1Wire.hpp
 *
 * @date 14.12.2015
 * @author Andre
 * @description
 */
#ifndef DALLAS_1WIRE_HPP_
#define DALLAS_1WIRE_HPP_

#include "stm32f0xx.h"
#include <stdint.h>
#include "tools/systimer.h"

class Dallas1Wire
{
public:
  Dallas1Wire(GPIO_TypeDef *dataGPIO, uint16_t dataPin);
  virtual ~Dallas1Wire() { }

  static char calcCRC(const char *data, unsigned int size);

protected:
  int reset();
  void writeByte(char byte);
  bool readBit();
  char readByte();

  GPIO_TypeDef* _dataGPIO;
  uint16_t _dataPin;

private:
  void setBusDirection(bool output);
  void setBusState(bool high);
  bool getBusState();
  void writeBit(bool bit);
  unsigned int getPinPos(uint16_t pin);

  unsigned int _pinPos;
};

#endif /* DALLAS_1WIRE_HPP_ */
