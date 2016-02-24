/**
 * @file gpio.h
 *
 * @date 24.02.2016
 * @author Andre
 * @description
 */
#ifndef GPIO_H_
#define GPIO_H_

#include <stdint.h>
#include "hw_config.h"

typedef struct _Gpio
{
  GPIO_TypeDef *gpioPort;
  uint16_t gpioPin;
} Gpio;

#endif /* GPIO_H_ */
