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
  _spi->setBits(8);
  _spi->setFirstBitLSB(true);
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

    delay_ms(20);

    // set Reset to high
    _gpioReset.gpioPort->BSRR = _gpioReset.gpioPin;
  }
}

int DOGS104_SPI::writeCommand(uint16_t command)
{
  _spi->reconfigure();

  char data[3];

  // Transfer "from left to right"

  // First byte: 1 1 1 1 1 R/W RS 0
  data[0] = 0x1F | ((command & 0x300) >> 3);

  // Second byte: D0 D1 D2 D3 0 0 0 0
  data[1] = command & 0x0F;

  // Third byte: D4 D5 D6 D7 0 0 0 0
  data[2] = (command & 0xF0) >> 4;

  // Transfer 3 bytes

  _spi->select();

  for (unsigned int i = 0; i < sizeof(data); i++)
  {
  _spi->transfer(data[i]);
  }

  _spi->unselect();

  return 0;
}

char DOGS104_SPI::read(bool rs)
{
//  _spi->reconfigure();

  char data[2];

  // Transfer "from left to right"

  // First byte: 1 1 1 1 1 R/W RS 0 (R = 1)
  data[0] = 0x3F | ((rs == true) ? 0x40 : 0x00);
  data[1] = 0;

  // Transfer 2 bytes

  _spi->select();

  _spi->transfer(data[0]);
  char result = _spi->transfer(data[1]);

  _spi->unselect();

  return result;
}
