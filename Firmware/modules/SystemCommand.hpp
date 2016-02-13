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
    setpointCurrent = 0.01;
  }

  // setpoint current
  float setpointCurrent;

  TwoPointCalibration calibSetpointCurrent;

private:
  NVRAM* _nvRAM;
};

#endif /* SYSTEMCOMMAND_HPP_ */
