/** $Id:  $
  * \file
  * \author  $Author:  $
  * \version $Revision:  $
  * \package ISM-LabPowerSupply
  *
  *
  *
  * (C) 2015 by André Heßling
  */

/*----------------------------------------------------------------------------*\
** Include files                                                              **
**                                                                            **
\*----------------------------------------------------------------------------*/
#include "hw_config.h"
#include "systimer.h"
#include <stdio.h>
#include <string.h>
#include "driver/spiF0.hpp"
#include "modules/HMI_USB.hpp"
#include "modules/OutputControl.hpp"
#include "modules/InputSense.hpp"
#include "modules/NVRAM.hpp"
#include "modules/HMI_Front.hpp"


#ifdef SEMIHOST
#include <stdio.h>
extern void  initialise_monitor_handles(void);
#endif

/*----------------------------------------------------------------------------*\
** Definitions                                                                **
**                                                                            **
\*----------------------------------------------------------------------------*/


/*----------------------------------------------------------------------------*\
** Variables                                                                  **
**                                                                            **
\*----------------------------------------------------------------------------*/


/*----------------------------------------------------------------------------*\
** Functions                                                                  **
**                                                                            **
\*----------------------------------------------------------------------------*/

int main(void)
{
  mstimer_init();
  ustimer_init();

  // Wait some time for LCD to power up
  delay_ms(100);

  RCC->AHBENR |= RCC_AHBPeriph_GPIOA | RCC_AHBPeriph_GPIOB | RCC_AHBPeriph_GPIOC | RCC_AHBPeriph_GPIOD;

  // low level hardware modules
  SPI spiDisplay(DISPLAY_SPI);

  // modules
  NVRAM nvRAM;
  SystemState systemState(&nvRAM);
  SystemCommand systemCommand(&nvRAM);
  OutputControl outputControl;
  InputSense inputSense;
  HMI_USB hmiUSB(&nvRAM);
  HMI_Front hmiFront(&spiDisplay);

  for (;;)
  {
    // Module: Input sense (get actual system state)
    inputSense.execute(systemCommand, systemState);

    // Module: USB interface
    hmiUSB.execute(systemState, systemCommand);

    // Module: HMI Front (display and rotary encoder)
    hmiFront.execute(systemState, systemCommand);

    // Module: Output control
    outputControl.execute(systemState, systemCommand);
  }

  return 0;
}
