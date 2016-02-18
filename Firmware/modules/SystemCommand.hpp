/**
 * @file SystemCommand.hpp
 *
 * @date 11.12.2015
 * @author Andre
 * @description
 */
#ifndef SYSTEMCOMMAND_HPP_
#define SYSTEMCOMMAND_HPP_

#include "tools/TwoPointCalibration.hpp"
#include "NVRAM.hpp"

// Current increase/decrease per step [A]
#define SETPOINT_CURRENT_STEP       0.01

#define CURRENT_LIMIT_LOW           0 // A
#define CURRENT_LIMIT_HIGH          3 // A

class SystemCommand
{
public:
  SystemCommand()
  {
    resetToSafeState();
  }

  SystemCommand(NVRAM* nvRAM) : SystemCommand()
  {
    _nvRAM = nvRAM;

    // load calibration data
    _nvRAM->loadMemorySlot(NVRAM::MemoryLayout::CalibSetpointCurrent, calibSetpointCurrent);
  }

  virtual ~SystemCommand() { }

  void resetToSafeState()
  {
    setpointCurrent = 0.f;
  }

  /** Change the setpoint current step-wise.
   *
   * The system limits are taken into account.
   *
   * @param steps Number of steps to increment/decrement
   */
  void stepSetpointCurrent(int steps)
  {
    setpointCurrent += steps * SETPOINT_CURRENT_STEP;

    if (setpointCurrent > CURRENT_LIMIT_HIGH)
    {
      setpointCurrent = CURRENT_LIMIT_HIGH;
    }
    else if (setpointCurrent < CURRENT_LIMIT_LOW)
    {
      setpointCurrent = CURRENT_LIMIT_LOW;
    }
  }

  // setpoint current
  float setpointCurrent;

  TwoPointCalibration calibSetpointCurrent;

private:
  NVRAM* _nvRAM;
};

#endif /* SYSTEMCOMMAND_HPP_ */
