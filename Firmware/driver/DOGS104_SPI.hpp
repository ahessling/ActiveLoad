/**
 * @file DOGS104_SPI.hpp
 *
 * @date 24.02.2016
 * @author Andre
 * @description
 */
#ifndef DOGS104_SPI_HPP_
#define DOGS104_SPI_HPP_

#include "DOGS104.hpp"
#include "driver/spibase.hpp"
#include "gpio.h"

class DOGS104_SPI : public DOGS104
{
public:
  DOGS104_SPI(SPIBase* spi, const Gpio& gpioCS,
      const Gpio& gpioReset,
      enum DogFontWidth fontWidth, enum DogDisplayLines lines,
        bool topView = false);
  virtual ~DOGS104_SPI() { }

  char read(bool rs);

private:
  void reset();

  int writeCommand(uint16_t command);

  SPIBase* _spi;
  const Gpio _gpioReset;
};

#endif /* DOGS104_SPI_HPP_ */
