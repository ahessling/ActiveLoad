/**
 * @file hw_config.c
 *
 * @date 27.04.2014
 * @author Andre
 * @description
 */

#include "hw_config.h"

/** Jump to a memory address and execute application from there.
 *
 * All peripheral clocks will be disabled.
 *
 * @param appaddr New application address
 */
void jump_to(uint32_t appaddr)
{
  volatile uint32_t jumpaddr;
  void (*app_fn)(void) = 0;

  // prepare jump address
  jumpaddr = *(__IO uint32_t*) (appaddr + 4);

  // prepare jumping function
  app_fn = (void (*)(void)) jumpaddr;

  // disable all tasks and clocks
  RCC_DeInit();
  SysTick->CTRL = 0;
  SysTick->LOAD = 0;
  SysTick->VAL = 0;

  // reset and disable all peripheral clocks (except for SRAM/FLITF)
  RCC->APB1RSTR = 0xFFFFFFFF;
  RCC->APB1RSTR = 0;
  RCC->APB2RSTR = 0xFFFFFFFF;
  RCC->APB2RSTR = 0;
  RCC->AHBRSTR = 0xFFFFFFEB;
  RCC->AHBRSTR = 0;

  RCC->AHBENR = 0x00000014;
  RCC->APB1ENR = 0;
  RCC->APB2ENR = 0;

  // switch back to main stack pointer
  __set_CONTROL(0x0);

  // re-init main stack pointer
  __set_MSP(*(__IO uint32_t*) appaddr);

  // jump.
  app_fn();
}
