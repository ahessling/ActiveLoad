/**
 * @file InputSense.cpp
 *
 * @date 19.12.2015
 * @author Andre
 * @description
 */
#include "InputSense.hpp"
#include "hw_config.h"
#include "tools/systimer.h"
#include <stdio.h>

#define TEMPERATURE_INTERVAL        5000 // ms
#define OVERTEMPERATURE_LIMIT       70.0 // Celsius

#define VOLTAGE_FILTER_LENGTH       32
#define CURRENT_FILTER_LENGTH       32

InputSense::InputSense()
  : _tempPower(DS18B20_POWER_PORT, DS18B20_POWER_PIN),
    _filterActualVoltage(VOLTAGE_FILTER_LENGTH),
    _filterActualCurrent(CURRENT_FILTER_LENGTH)
{
  // Force initial temperature update (take into account that the result takes some time)
  _tempPower.startConversion();
  _timeLastTemperature = -TEMPERATURE_INTERVAL + 2000;

  lowLevelInit();
}

void InputSense::lowLevelInit()
{
  // init internal ADC

  // ADC+DMA clock configuration
  RCC->AHBENR |= RCC_AHBPeriph_DMA1;
  RCC->APB2RSTR |= RCC_APB2Periph_ADC1;
  RCC->APB2RSTR &= ~RCC_APB2Periph_ADC1;
  RCC->APB2ENR |= RCC_APB2Periph_ADC1;

  // enable HSI14 for ADCCLK and wait for it
  RCC->CR2 |= RCC_CR2_HSI14ON;
  while ((RCC->CR2 & RCC_CR2_HSI14RDY) == 0);

  // ADC GPIO configuration
  ADC_VOLTAGE_PORT->MODER |= 0x03 << (2 * ADC_VOLTAGE_SOURCE); // Analog mode
  ADC_VOLTAGE_PORT->PUPDR &= ~(0x03 << (2 * ADC_VOLTAGE_SOURCE)); // No pullup/pulldown
  ADC_VOLTAGE_PORT->MODER |= 0x03 << (2 * ADC_CURRENT_SOURCE); // Analog mode
  ADC_VOLTAGE_PORT->PUPDR &= ~(0x03 << (2 * ADC_CURRENT_SOURCE)); // No pullup/pulldown

  // ADC configuration
  ADC1->CFGR1 = ADC_CFGR1_CONT; // 12 bit, continuous mode, DMA

  // set channels and sampling time (current --> voltage --> current...)
  ADC1->CHSELR = ADC_CURRENT_CHANNEL | ADC_VOLTAGE_CHANNEL;
  ADC1->SMPR = ADC_SampleTime_71_5Cycles; // 6 µs sample+conversion

  // calibrate ADC
  ADC_GetCalibrationFactor(ADC1);

  // enable circular DMA
  ADC1->CFGR1 |=  ADC_CFGR1_DMACFG | ADC_CFGR1_DMAEN;

  // enable ADC and wait for ready flag
  ADC1->CR |= ADC_CR_ADEN;
  while (!(ADC1->ISR & ADC_ISR_ADRDY));

  // start ADC
  ADC1->CR |= ADC_CR_ADSTART;

  // init ADC DMA

  // circular mode, peripheral --> memory, 16-bit transfers
  // memory increment, high priority
  ADC_DMA_CHANNEL->CCR = DMA_CCR_CIRC | DMA_CCR_PSIZE_0 |
      DMA_CCR_MSIZE_0 | DMA_CCR_PL_1 | DMA_CCR_MINC;

  // memory addresses
  ADC_DMA_CHANNEL->CMAR = (uint32_t)&_adcVoltageRaw[0];
  ADC_DMA_CHANNEL->CPAR = (uint32_t)&(ADC1->DR);
  ADC_DMA_CHANNEL->CNDTR = 2; // 2 channels

  // enable DMA
  ADC_DMA_CHANNEL->CCR |= DMA_CCR_EN;
}

void InputSense::execute(SystemCommand& systemCommand,
    SystemState& systemState)
{
  systemState.setpointCurrent = systemCommand.setpointCurrent;

  // execute actual voltage and current filter using EMA implementation
  _actualCurrent = _filterActualCurrent.execute(_adcVoltageRaw[0]); // 0..4095
  _actualVoltage = _filterActualVoltage.execute(_adcVoltageRaw[1]); // 0..4095

  // get actual current
  systemState.actualCurrent = systemState.calibActualCurrent.translate(getActualCurrent());

  // current cannot be negative
  if (systemState.actualCurrent < 0)
  {
    systemState.actualCurrent = 0;
  }

  // get actual voltage
  systemState.actualVoltage = systemState.calibActualVoltage.translate(getActualVoltage());

  // voltage cannot be negative
  if (systemState.actualVoltage < 0)
  {
    systemState.actualVoltage = 0;
  }

  // calculate power
  systemState.actualPower = systemState.actualVoltage * systemState.actualCurrent;

  // get temperatures every 5 seconds
  if (mstimer_get() - _timeLastTemperature > TEMPERATURE_INTERVAL)
  {
    float temp;

    // get temperature from previous conversion
    if (_tempPower.readTemperature(&temp) == 0)
    {
      systemState.temperaturePower = temp;

      // check for overtemperature condition
      if (systemState.temperaturePower >= OVERTEMPERATURE_LIMIT)
      {
        systemState.overtemperature = true;
      }
      else
      {
        systemState.overtemperature = false;
      }
    }
    else
    {
      // could not read temperature
      systemState.temperaturePower = NAN;
    }

    // start new conversions
    _tempPower.startConversion();

    _timeLastTemperature = mstimer_get();
  }
}

float InputSense::getActualVoltage()
{
  // calculate voltage at input
  float voltage = 3.30 / 4096.0 * (float)_actualVoltage;

  // take voltage divider into account (56k / 4k7)
  voltage *= 12.91;

  return voltage;
}

float InputSense::getActualCurrent()
{
  // calculate voltage at input
  float current = 3.30 / 4096.0 * (float)_actualCurrent;

  // about 1 mV / mA, so no transformation

  return current;
}
