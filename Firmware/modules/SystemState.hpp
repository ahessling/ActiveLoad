/**
 * @file SystemState.hpp
 *
 * @date 11.12.2015
 * @author Andre
 * @description
 */
#ifndef SYSTEMSTATE_HPP_
#define SYSTEMSTATE_HPP_

#include "tools/TwoPointCalibration.hpp"
#include "NVRAM.hpp"

class SystemState
{
public:
  SystemState(NVRAM* nvRAM)
  {
    // init these in case the communication fails
    temperaturePower = 0.0;

    _nvRAM = nvRAM;

    // load calibration data
    _nvRAM->loadMemorySlot(NVRAM::MemoryLayout::CalibActualVoltage, calibActualVoltage);
    _nvRAM->loadMemorySlot(NVRAM::MemoryLayout::CalibActualCurrent, calibActualCurrent);
  }

  virtual ~SystemState() { }

  // setpoint and actual voltage/current
  float setpointCurrent;
  float actualVoltage;
  float actualCurrent;
  TwoPointCalibration calibActualVoltage;
  TwoPointCalibration calibActualCurrent;

  // temperature
  float temperaturePower;

private:
  NVRAM* _nvRAM;
};

#endif /* SYSTEMSTATE_HPP_ */
