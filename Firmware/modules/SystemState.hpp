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
#include <math.h>

class SystemState
{
public:
  SystemState(NVRAM* nvRAM)
  {
    // init these in case the communication fails
    temperaturePower = NAN;
    overtemperature = false;

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
  float actualPower; ///< Actual power: actualVoltage * actualCurrent

  TwoPointCalibration calibActualVoltage;
  TwoPointCalibration calibActualCurrent;

  // temperature
  float temperaturePower;

  bool overtemperature; ///< Overtemperature condition

  bool operator!=(const SystemState& rhs) const
  {
    bool unequal = false;

    unequal |= setpointCurrent != rhs.setpointCurrent;
    unequal |= actualVoltage != rhs.actualVoltage;
    unequal |= actualCurrent != rhs.actualCurrent;
    unequal |= actualPower != rhs.actualPower;
    unequal |= temperaturePower != rhs.temperaturePower;
    unequal |= overtemperature != rhs.overtemperature;

    return unequal;
  }

  bool operator==(const SystemState& rhs) const
  {
    return !(*this != rhs);
  }

private:
  NVRAM* _nvRAM;
};

#endif /* SYSTEMSTATE_HPP_ */
