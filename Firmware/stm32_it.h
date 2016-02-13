/**
 * @file stm32_it.h
 *
 * @date 08.12.2015
 * @author Andre
 * @description
 */
#ifndef STM32_IT_H_
#define STM32_IT_H_

#ifdef __cplusplus
 extern "C" {
#endif

#include "usb_dcd_int.h"

void USB_IRQHandler(void);

#ifdef __cplusplus
}
#endif

#endif /* STM32_IT_H_ */
