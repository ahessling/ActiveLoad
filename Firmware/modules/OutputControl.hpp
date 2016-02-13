/**
 * @file OutputControl.hpp
 *
 * @date 16.12.2015
 * @author Andre
 * @description
 */
#ifndef OUTPUTCONTROL_HPP_
#define OUTPUTCONTROL_HPP_

#include "SystemState.hpp"
#include "SystemCommand.hpp"
#include "driver/spibase.hpp"

class OutputControl
{
public:
  OutputControl();
  virtual ~OutputControl() { }

  void execute(SystemState& systemState, const SystemCommand& systemCommand,
      bool forceUpdate = false);

private:
  void lowLevelInit();
  void setSetpointCurrent(float current);

  SystemCommand _oldSystemCommand;
};

#endif /* OUTPUTCONTROL_HPP_ */
