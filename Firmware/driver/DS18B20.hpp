/**
 * @file DS18B20.hpp
 *
 * @date 14.12.2015
 * @author Andre
 * @description
 */
#ifndef DS18B20_HPP_
#define DS18B20_HPP_

#include "Dallas1Wire.hpp"
#include <stdint.h>

class DS18B20 : Dallas1Wire
{
public:
  DS18B20(GPIO_TypeDef *dataGPIO, uint16_t dataPin);
  virtual ~DS18B20() { }

  int startConversion(bool wait = false);

  int readTemperature(float *temperature);

  typedef enum _TemperatureSensor
  {
    Unknown = 0,
    DS18B20Type,
    DS18S20Type
  } TemperatureSensor;

private:
  int readScratchpad();
  int busyWait();
  int readROM();

  typedef struct __attribute__((packed))
  {
    uint8_t tempLow;
    uint8_t tempHigh;
    uint8_t user1;
    uint8_t user2;
    uint8_t config;
    uint8_t reserved[3];
    uint8_t crc;
  } DS18B20Scratchpad;

  DS18B20Scratchpad _scratchpad;
  bool _scratchpadValid;
  bool _conversionStarted;
  TemperatureSensor _temperatureSensor;
  char _romCode[8];
};

#endif /* DS18B20_HPP_ */
