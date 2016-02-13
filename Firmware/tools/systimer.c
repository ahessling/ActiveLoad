/** $Id:  $
  * \file
  * \author  $Author:  $
  * \version $Revision:  $
  * \project RFM12-OOK
  *
  *
  *
  * (C) 2013 by André Heßling
  */

/*----------------------------------------------------------------------------*\
** Include files                                                              **
**                                                                            **
\*----------------------------------------------------------------------------*/

#include "systimer.h"
#include "core_cm0.h"
#include "hw_config.h"

/*----------------------------------------------------------------------------*\
** Definitions                                                                **
**                                                                            **
\*----------------------------------------------------------------------------*/


/*----------------------------------------------------------------------------*\
** Variables                                                                  **
**                                                                            **
\*----------------------------------------------------------------------------*/
volatile uint32_t uptime_us = 0; ///< Uptime in us (32 Bit, läuft alle 71 Minuten über)
volatile uint32_t uptime_ms = 0; ///< Uptime in ms

volatile uint64_t total_uptime_us = 0; ///< Uptime in us (64 Bit)

/*----------------------------------------------------------------------------*\
** Functions                                                                  **
**                                                                            **
\*----------------------------------------------------------------------------*/
EXTERN void TIM2_IRQHandler()
{
  if (TIM_GetITStatus(TIM2, TIM_IT_Update))
  {
    uptime_us += 0x10000;
    total_uptime_us += 0x10000;

    // Clear update interrupt bit
    TIM_ClearITPendingBit(TIM2, TIM_FLAG_Update);
  }
}

EXTERN void SysTick_Handler()
{
	uptime_ms++;
}

/** usTimer auslesen.
 *
 * Liest den aktuellen Zählerstand des mikrosekunden-Timers aus.
 * Der Timer speichert die Anzahl der mikrosekunden seit Initialisierung und
 * läuft alle 70 Minuten über.
 *
 * \return Mikrosekunden seit Initialisierung.
 */
uint32_t ustimer_get()
{
  uint32_t v1, v2, tim;
  do
  {
    // Uptime auslesen
    v1 = uptime_us;

    // Timer auslesen
    tim = TIM2->CNT;

    // Uptime nochmal auslesen
    v2 = uptime_us;

    // Wenn sich die Uptime geändert hat, ist irgendwann TIMx umgekippt
    // und wir wissen nicht, ob TIMx vorher oder hinterher ausgelesen wurde.
    // Daher solange wiederholen, bis ein gültiger Wert vorliegt.
  } while (v1 != v2);

  return v1 + tim;
}

/** Mikrosekunden-Timer initialisieren.
 *
 * Initialisiert den Mikrosekundentimer. Hierbei wird der Timer so konfiguriert,
 * dass er ein Inkrement pro us macht und nach 65535us überläuft und dabei einen
 * Interrupt auslöst. Dieser zählt die Variable uptime hoch.
 */
void ustimer_init()
{
  RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM2, ENABLE);

  /* Set up timer interrupt */
  NVIC_InitTypeDef NVIC_InitStructure = {
      .NVIC_IRQChannel = TIM2_IRQn,
      .NVIC_IRQChannelPriority = 1,
      .NVIC_IRQChannelCmd = ENABLE
  };

  NVIC_Init(&NVIC_InitStructure);

  /* Timer auf 1us initialisieren. */
  TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure = {
      .TIM_Prescaler = (uint16_t)((SystemCoreClock / 1000000) - 1),
      .TIM_CounterMode = TIM_CounterMode_Up,
      .TIM_Period = 65535,
      .TIM_ClockDivision = TIM_CKD_DIV1,
      .TIM_RepetitionCounter = 0
  };

  TIM_TimeBaseInit(TIM2, &TIM_TimeBaseStructure);

  /* Clear update interrupt bit */
  TIM_ClearITPendingBit(TIM2, TIM_FLAG_Update);

  /* Enable update interrupt  */
  TIM_ITConfig(TIM2, TIM_FLAG_Update,ENABLE);

  /* TIM enable counter */
  TIM_Cmd(TIM2, ENABLE);
}

/**
 * Perform a microsecond delay
 *
 * \param  us  number of microseconds to wait.
 */
void delay_us(unsigned us)
{
  uint32_t start = ustimer_get();
  while (ustimer_get() - start < us);
}

/**
 * Perform a millisecond delay
 *
 * \param  ms  number of milliseconds to wait.
 */
void delay_ms(unsigned ms)
{
    delay_us(ms * 1000);
}

/** Initializes the millisecond timer */
void mstimer_init(void)
{
	SysTick_CLKSourceConfig(SysTick_CLKSource_HCLK);
	SysTick_Config(SystemCoreClock / 1000);
}

/** Return the number of milliseconds since start.
 *
 * @return Milliseconds
 */
uint32_t mstimer_get(void)
{
  return uptime_ms;
}

/** Perform a millisecond delay based on the millisecond timer.
 *
 * @param ms number of milliseconds to wait
 */
void mstimer_delay(uint32_t ms)
{
  uint32_t start = mstimer_get();
  while (mstimer_get() - start < ms);
}

/** Return the number of microseconds since start.
 *
 * This function returns a 64 bit value.
 *
 * @return Microseconds
 */
uint64_t get_uptime_us()
{
  return total_uptime_us;
}

/** Return the number of milliseconds since start.
 *
 * This function returns a 64 bit value.
 *
 * @return milliseconds
 */
uint64_t get_uptime_ms()
{
  return total_uptime_us / 1000;
}
