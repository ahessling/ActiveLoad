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

#define CURRENT_LIMIT_LOW           0.0 // A
#define CURRENT_LIMIT_HIGH          3.0 // A

class SystemCommand
{
public:
  enum SystemCommandError
  {
    OK = 0,
    ValueLowerLimit = -1,
    ValueUpperLimit = -2
  };

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

  /** Change the setpoint current.
   *
   * The system limits are taken into account.
   *
   * @param current New setpoint current
   * @return 0 if OK, otherwise @see SystemCommandError
   */
  enum SystemCommandError setSetpointCurrent(float current)
  {
    if (current > CURRENT_LIMIT_HIGH)
    {
      return ValueUpperLimit;
    }
    else if (current < CURRENT_LIMIT_LOW)
    {
      return ValueLowerLimit;
    }

    // apply new setting
    setpointCurrent = current;

    return OK;
  }

  // setpoint current
  float setpointCurrent;

  TwoPointCalibration calibSetpointCurrent;

private:
  NVRAM* _nvRAM;
};

#endif /* SYSTEMCOMMAND_HPP_ */
