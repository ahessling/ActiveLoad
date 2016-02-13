/**
 * @file InputSense.hpp
 *
 * @date 19.12.2015
 * @author Andre
 * @description
 */
#ifndef INPUTSENSE_HPP_
#define INPUTSENSE_HPP_

#include "SystemState.hpp"
#include "SystemCommand.hpp"
#include "driver/DS18B20.hpp"
#include <stdint.h>
#include "tools/EMAFilter.hpp"

class InputSense
{
public:
  InputSense();
  virtual ~InputSense() { }

  void lowLevelInit();
  void execute(SystemCommand& systemCommand, SystemState& systemState);

private:
  float getActualVoltage();
  float getActualCurrent();

  DS18B20 _tempPower;
  unsigned int _timeLastTemperature;
  volatile uint16_t _adcVoltageRaw[2];
  EMAFilter _filterActualVoltage;
  EMAFilter _filterActualCurrent;
  float _actualVoltage;
  float _actualCurrent;
};


#endif /* INPUTSENSE_HPP_ */
