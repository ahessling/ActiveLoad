/**
 * @file DOGS104_SPI.cpp
 *
 * @date 24.02.2016
 * @author Andre
 * @description
 */
#include "DOGS104_SPI.hpp"
#include "tools/systimer.h"

DOGS104_SPI::DOGS104_SPI(SPIBase* spi,
    const Gpio& gpioCS, const Gpio& gpioReset,
    enum DogFontWidth fontWidth, enum DogDisplayLines lines,
    bool topView) :
    DOGS104(fontWidth, lines, topView),
    _spi(spi), _gpioReset(gpioReset)
{
  // setup SPI
  _spi->configureCS(gpioCS.gpioPort, gpioCS.gpioPin);
  _spi->setBits(16);
  _spi->setCPOL(true);
  _spi->setCPHA(true);
  _spi->setPrescaler(5); // 750 kHz
  _spi->init();
}

void DOGS104_SPI::reset()
{
  if (0 != _gpioReset.gpioPort)
  {
    // set Reset to low
    _gpioReset.gpioPort->BRR = _gpioReset.gpioPin;

    delay_ms(10);

    // set Reset to high
    _gpioReset.gpioPort->BSRR = _gpioReset.gpioPin;
  }
}

int DOGS104_SPI::writeCommand(uint16_t command)
{
  _spi->reconfigure();

  _spi->select();

  _spi->transfer(command);

  _spi->unselect();

  return 0;
}
