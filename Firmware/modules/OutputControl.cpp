/**
 * @file OutputControl.cpp
 *
 * @date 16.12.2015
 * @author Andre
 * @description
 */
#include "OutputControl.hpp"
#include "hw_config.h"

OutputControl::OutputControl()
{
  // init low level hardware
  lowLevelInit();
}

void OutputControl::execute(SystemState& systemState,
    const SystemCommand& systemCommand, bool forceUpdate)
{
  // check if there is work to do

  // setpoint current
  if (systemCommand.setpointCurrent != _oldSystemCommand.setpointCurrent || forceUpdate)
  {
    // set new setpoint current
    // use linear correction if calibrated
    float calibCurrent = systemCommand.calibSetpointCurrent.translate(systemCommand.setpointCurrent);

    // if current is 0, turn off DAC to minimize current draw
    if (systemCommand.setpointCurrent < 0.001)
    {
      disconnectDAC();
    }
    else
    {
      connectDAC();
    }

    setSetpointCurrent(calibCurrent);
  }

  // save old command state
  _oldSystemCommand = systemCommand;
}

void OutputControl::lowLevelInit()
{
  // DAC GPIO configuration
  DAC_CURRENT_PORT->MODER |= 0x03 << (2 * DAC_CURRENT_SOURCE); // Analog mode
  DAC_CURRENT_PORT->PUPDR &= ~(0x03 << (2 * DAC_CURRENT_SOURCE)); // No pullup/pulldown

  // enable DAC clock
  RCC->APB1RSTR |= RCC_APB1Periph_DAC;
  RCC->APB1RSTR &= ~RCC_APB1Periph_DAC;
  RCC->APB1ENR |= RCC_APB1Periph_DAC;

  // reset to 0V
  DAC->DHR12R1 = 0;
}

/** Change setpoint current.
 *
 * @param current Current in A
 */
void OutputControl::setSetpointCurrent(float current)
{
  // current must not be negative
  if (current < 0)
  {
    current = 0;
  }

  // calculate DAC value

  // gain of 1 (assuming 0.1 Ohm shunt and "DAC to opamp" gain of 0.1)
  float voltage = current;

  unsigned int dacValue = voltage / (3.30f / 4096.f);

  // limit to 12 bits
  if (dacValue > 0xFFF)
  {
    dacValue = 0xFFF;
  }

  DAC->DHR12R1 = dacValue;
}

/** Turn off DAC.
 *
 */
void OutputControl::disconnectDAC()
{
  // disconnect DAC by switching it off
  DAC->DHR12R1 = 0;
  DAC->CR = 0;

  // set port pin to LOW
  DAC_CURRENT_PORT->BRR = DAC_CURRENT_PIN;
}

/** Turn on DAC.
 *
 */
void OutputControl::connectDAC()
{
  // init and enable DAC output 1: Output buffer disabled, no trigger
  DAC->CR = DAC_CR_EN1 | DAC_CR_BOFF1;
}
