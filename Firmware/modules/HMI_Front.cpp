/**
 * @file HMI_Front.cpp
 *
 * @date 13.02.2016
 * @author Andre
 * @description
 */

#include "HMI_Front.hpp"
#include "hw_config.h"

HMI_Front::HMI_Front(SPIBase* spi) : _spi(spi)
{
  lowLevelInit();
}

bool HMI_Front::execute(SystemState& systemState, SystemCommand& systemCommand)
{
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
}
