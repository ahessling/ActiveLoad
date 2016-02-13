/**
 * @file stm32_it.cpp
 *
 * @date 08.12.2015
 * @author Andre
 * @description
 */

#include "hw_config.h"
#include "stm32_it.h"

/** ISR für Bittimings */
EXTERN void USB_IRQHandler(void)
{
  USB_Istr();
}
