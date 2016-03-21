/** $Id:  $
  * \file
  * \author  $Author:  $
  * \version $Revision:  $
  * \package LabPowerSupply
  *
  *
  *
  * (C) 2014 by André Heßling
  */

#ifndef HW_CONFIG_H_
#define HW_CONFIG_H_

/*----------------------------------------------------------------------------*\
** Include files                                                              **
**                                                                            **
\*----------------------------------------------------------------------------*/
#include "stm32f0xx.h"
#include "stm32f0xx_conf.h"

#ifdef __cplusplus
#define EXTERN extern "C"
#else
#define EXTERN
#endif

/*----------------------------------------------------------------------------*\
** Definitions                                                                **
**                                                                            **
\*----------------------------------------------------------------------------*/
/** @addtogroup /LabPowerSupply/hw_config.h
  * @{
  */

#define USE_SHELL

#ifdef USE_SHELL
#define SHELL_INTERACTIVE
#define SHELL_USART 1
#define SHELL_BAUDRATE 115200
#endif

#define USB_CLOCK_SOURCE_CRS
#define USB_IT_PRIO                    1

// Display
#define DISPLAY_SPI             SPI2
#define DISPLAY_SPI_AF          GPIO_AF_0
//#define DAC_SPI_PRESCALER
#define DISPLAY_SCK_PORT        GPIOB
#define DISPLAY_SCK_PIN         GPIO_Pin_13
#define DISPLAY_SCK_SOURCE      GPIO_PinSource13
#define DISPLAY_MISO_PORT       GPIOB
#define DISPLAY_MISO_PIN        GPIO_Pin_14
#define DISPLAY_MISO_SOURCE     GPIO_PinSource14
#define DISPLAY_MOSI_PORT       GPIOB
#define DISPLAY_MOSI_PIN        GPIO_Pin_15
#define DISPLAY_MOSI_SOURCE     GPIO_PinSource15
#define DISPLAY_CS_PORT         GPIOB
#define DISPLAY_CS_PIN          GPIO_Pin_12
#define DISPLAY_RESET_PORT      GPIOB
#define DISPLAY_RESET_PIN       GPIO_Pin_11

// Internal ADC (actual voltage)
#define ADC_VOLTAGE_CHANNEL     ADC_Channel_3
#define ADC_VOLTAGE_PORT        GPIOA
#define ADC_VOLTAGE_PIN         GPIO_Pin_3
#define ADC_VOLTAGE_SOURCE      GPIO_PinSource3
#define ADC_CURRENT_CHANNEL     ADC_Channel_0
#define ADC_CURRENT_PORT        GPIOA
#define ADC_CURRENT_PIN         GPIO_Pin_0
#define ADC_CURRENT_SOURCE      GPIO_PinSource0
#define ADC_DMA                 DMA1
#define ADC_DMA_CHANNEL         DMA1_Channel1

// Internal DAC (setpoint current)
#define DAC_CURRENT_PORT        GPIOA
#define DAC_CURRENT_PIN         GPIO_Pin_4
#define DAC_CURRENT_SOURCE      GPIO_PinSource4

// Temperature sensors (DS18B20)
#define DS18B20_POWER_PORT      GPIOA
#define DS18B20_POWER_PIN       GPIO_Pin_10

// Rotary encoder
#define ENCODER_A_PORT          GPIOA
#define ENCODER_A_PIN           GPIO_Pin_6
#define ENCODER_A_SOURCE        GPIO_PinSource6
#define ENCODER_B_PORT          GPIOA
#define ENCODER_B_PIN           GPIO_Pin_7
#define ENCODER_B_SOURCE        GPIO_PinSource7
#define ENCODER_SWITCH_PORT     GPIOA
#define ENCODER_SWITCH_PIN      GPIO_Pin_2
#define ENCODER_TIM             TIM3
#define ENCODER_AF              GPIO_AF_1

// LED
#define LED_STATE_PORT          GPIOB
#define LED_STATE_PIN           GPIO_Pin_0

/*----------------------------------------------------------------------------*\
** Structures                                                                 **
**                                                                            **
\*----------------------------------------------------------------------------*/


/*----------------------------------------------------------------------------*\
** Variables                                                                  **
**                                                                            **
\*----------------------------------------------------------------------------*/


/*----------------------------------------------------------------------------*\
** Function prototypes                                                        **
**                                                                            **
\*----------------------------------------------------------------------------*/
void jump_to(uint32_t appaddr);

/**
  * @}
  */

#endif /* HW_CONFIG_H_ */
