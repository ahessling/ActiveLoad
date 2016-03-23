/**
 * @file HMI_Front.cpp
 *
 * @date 13.02.2016
 * @author Andre
 * @description
 */

#include "HMI_Front.hpp"
#include "hw_config.h"
#include <stdio.h>
#include <stdlib.h>
#include "driver/gpio.h"
#include "tools/systimer.h"
#include <math.h>

// Minimum number of encoder steps between two setpoint current "steps"
#define ENCODER_STEPS_PER_TURN      2

// Minimum time between two display updates [ms]
#define DISPLAY_REFRESH_INTERVAL    100

HMI_Front::HMI_Front(SPIBase* spi) : _spi(spi),
  _display(spi,
  {DISPLAY_CS_PORT, DISPLAY_CS_PIN}, // CS
  {DISPLAY_RESET_PORT, DISPLAY_RESET_PIN}, // Reset
  DOGS104::DogFontWidth::FONT_WIDTH_5, DOGS104::DogDisplayLines::LINES_4)
{
  _blinkTimer = 0;
  _displayBlinkState = false;
  _ledBlinkState = false;

  lowLevelInit();

  _encoderCounter = ENCODER_TIM->CNT;
  _lastEncoderCounter = _encoderCounter;

  // init display and turn it on
  _display.init();
}

bool HMI_Front::execute(SystemState& systemState, SystemCommand& systemCommand)
{
  _encoderCounter = ENCODER_TIM->CNT;

  // detect encoder movement
  if (_encoderCounter != _lastEncoderCounter)
  {
    int16_t encoderDiff = _encoderCounter - _lastEncoderCounter;

    if (abs(encoderDiff) >= ENCODER_STEPS_PER_TURN)
    {
      // increase/decrease setpoint current
      systemCommand.stepSetpointCurrent(encoderDiff / ENCODER_STEPS_PER_TURN);

      _lastEncoderCounter = _encoderCounter;
    }
  }

  // limit display refresh time
  if (mstimer_get() - _lastDisplayUpdate > DISPLAY_REFRESH_INTERVAL)
  {
    // increment display timer state
    _blinkTimer++;

    // refresh LCD
    updateDisplay(systemState);

    // update LED
    updateLED(systemState);

    // save last refresh time
    _lastDisplayUpdate = mstimer_get();
  }

  // check if encoder key is pressed
  if (!(ENCODER_SWITCH_PORT->IDR & ENCODER_SWITCH_PIN))
  {
    // reset to safe state (setpoint current to 0)
    systemCommand.resetToSafeState();
  }

  return false;
}

void HMI_Front::lowLevelInit()
{
  // init display connection

  // alternate functions
  GPIO_PinAFConfig(DISPLAY_SCK_PORT, DISPLAY_SCK_SOURCE, DISPLAY_SPI_AF);
  GPIO_PinAFConfig(DISPLAY_MOSI_PORT, DISPLAY_MOSI_SOURCE, DISPLAY_SPI_AF);
  GPIO_PinAFConfig(DISPLAY_MISO_PORT, DISPLAY_MISO_SOURCE, DISPLAY_SPI_AF);

  // configure GPIO pins
  GPIO_InitTypeDef gpio;
  gpio.GPIO_Mode = GPIO_Mode_AF;
  gpio.GPIO_OType = GPIO_OType_PP;
  gpio.GPIO_PuPd  = GPIO_PuPd_DOWN;
  gpio.GPIO_Speed = GPIO_Speed_Level_3;

  gpio.GPIO_Pin = DISPLAY_SCK_PIN;
  GPIO_Init(DISPLAY_SCK_PORT, &gpio);

  gpio.GPIO_Pin = DISPLAY_MOSI_PIN;
  GPIO_Init(DISPLAY_MOSI_PORT, &gpio);

  gpio.GPIO_Pin = DISPLAY_MISO_PIN;
  GPIO_Init(DISPLAY_MISO_PORT, &gpio);

  gpio.GPIO_Mode = GPIO_Mode_OUT;
  gpio.GPIO_Pin = DISPLAY_CS_PIN;
  DISPLAY_CS_PORT->BSRR = DISPLAY_CS_PIN; // CS high, unselect
  GPIO_Init(DISPLAY_CS_PORT, &gpio);

  // Reset line
  gpio.GPIO_Pin = DISPLAY_RESET_PIN;
  DISPLAY_RESET_PORT->BSRR = DISPLAY_RESET_PIN; // Reset high (no reset)
  GPIO_Init(DISPLAY_RESET_PORT, &gpio);

  // init encoder interface
  gpio.GPIO_Pin = ENCODER_A_PIN;
  gpio.GPIO_Mode = GPIO_Mode_AF;
  gpio.GPIO_PuPd  = GPIO_PuPd_UP;
  GPIO_Init(ENCODER_A_PORT, &gpio);

  gpio.GPIO_Pin = ENCODER_B_PIN;
  GPIO_Init(ENCODER_B_PORT, &gpio);

  gpio.GPIO_Mode = GPIO_Mode_IN;
  gpio.GPIO_Pin = ENCODER_SWITCH_PIN;
  GPIO_Init(ENCODER_SWITCH_PORT, &gpio);

  // select alternate function
  GPIO_PinAFConfig(ENCODER_A_PORT, ENCODER_A_SOURCE, ENCODER_AF);
  GPIO_PinAFConfig(ENCODER_B_PORT, ENCODER_B_SOURCE, ENCODER_AF);

  // timer configuration

  // init timer peripheral clock
  RCC->APB1ENR |= RCC_APB1Periph_TIM3;

  // set time base
  ENCODER_TIM->CR1 = 2 << 8; // sampling clock of filters = timer_clock / 4
  ENCODER_TIM->PSC = 0; // 48 MHz (no prescaler)
  ENCODER_TIM->ARR = 0xFFFF; // use full 16 bit as period
  ENCODER_TIM->EGR = TIM_PSCReloadMode_Immediate; // immediate update

  // set encoder mode

  // Encoder mode 1: Counter counts up/down on TI2FP1 edge depending on TI1FP2 level
  ENCODER_TIM->SMCR = TIM_EncoderMode_TI1;

  // configure input capture filter for both inputs
  unsigned char filter = 7; // 0..15 (s. Reference manual p. 432); 7 = f_DTS / 4, N = 8

  // IC1 is mapped on TI1, IC2 is mapped on TI2
  ENCODER_TIM->CCMR1 = TIM_CCMR1_CC1S_0 | TIM_CCMR1_CC2S_0 | (filter << 4) | (filter << 12);

  // polarity: TI1FP1 rising edge, TI2FP1 rising edge
  ENCODER_TIM->CCER = TIM_ICPolarity_Rising | (TIM_ICPolarity_Rising << 4);

  // enable timer
  ENCODER_TIM->CR1 |= TIM_CR1_CEN;

  // LED
  gpio.GPIO_Mode = GPIO_Mode_OUT;
  gpio.GPIO_OType = GPIO_OType_PP;
  gpio.GPIO_Pin = LED_STATE_PIN;
  LED_STATE_PORT->BRR = LED_STATE_PIN;
  GPIO_Init(LED_STATE_PORT, &gpio);
}

/** Update the LCD.
 *
 * @param systemState Current SystemState
 */
void HMI_Front::updateDisplay(const SystemState& systemState)
{
  char line[12];

  // line 1
  sprintf(line, "Iact %1.2fA", systemState.actualCurrent);
  _display.write(line, 0, 0);

  // line 2
  sprintf(line, "Iset %1.2fA", systemState.setpointCurrent);
  _display.write(line, 0, 1);

  // line 3
  sprintf(line, "U   %4.1f V", systemState.actualVoltage);
  _display.write(line, 0, 2);

  // line 4
  if ((true == systemState.overtemperature) && (true == _displayBlinkState))
  {
    // empty line to "blink"
    sprintf(line, "          ");
  }
  else
  {
    // line 4
    if (!isnan(systemState.temperaturePower))
    {
      sprintf(line, "P %2dW/%2d C",
          (int)roundf(systemState.actualPower),
          (int)roundf(systemState.temperaturePower));
    }
    else
    {
      sprintf(line, "P %2dW/?  C",
          (int)roundf(systemState.actualPower));
    }
  }
  _display.write(line, 0, 3);

  // update blink state
  if ((_blinkTimer % 5) == 0)
  {
    _displayBlinkState = !_displayBlinkState;
  }
}

/** Update the LED.
 *
 * @param systemState Current SystemState
 */
void HMI_Front::updateLED(const SystemState& systemState)
{
  // update LED state
  if (true == systemState.overtemperature)
  {
    // blink very fast during overtemperature conditions
    if ((_blinkTimer % 5) == 0)
    {
      _ledBlinkState = !_ledBlinkState;
    }
  }
  else if (fabs(systemState.actualCurrent - systemState.setpointCurrent) > 0.1)
  {
    // blink slowly when setpoint and actual current differ
    if ((_blinkTimer % 10) == 0)
    {
      _ledBlinkState = !_ledBlinkState;
    }
  }
  else
  {
    // LED is on when everything is OK
    _ledBlinkState = true;
  }

  // set LED state
  if (true == _ledBlinkState)
  {
    // enable LED
    LED_STATE_PORT->BSRR = LED_STATE_PIN;
  }
  else
  {
    // disable LED
    LED_STATE_PORT->BRR = LED_STATE_PIN;
  }
}
