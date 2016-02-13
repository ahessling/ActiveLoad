/**
 * @file spibase.hpp
 *
 * @date 24.01.2015
 * @author Andre
 * @description
 */
#ifndef SPIBASE_HPP_
#define SPIBASE_HPP_

#include "stm32f0xx.h"

class SPIBase
{
public:
	SPIBase(SPI_TypeDef *spi);
	virtual ~SPIBase();

	void configureCS(GPIO_TypeDef *gpio, uint16_t csPin)
	{
		_csGPIO = gpio;
		_csPin = csPin;
	}

	void setCPOL(bool cpol)
	{
		_cpol = cpol;
	}

	void setCPHA(bool cpol)
	{
		_cpha = cpol;
	}

	void setFirstBitLSB(bool firstBitLSB)
	{
		_firstBitLSB = firstBitLSB;
	}

	virtual void setBits(unsigned char bits)
	{
		// support 8 and 16 bits mode
		if (8 == bits || 16 == bits)
			_bits = bits;
	}

	virtual void setPrescaler(uint16_t prescaler)
	{
		_prescaler = prescaler;
	}

	virtual bool init() = 0;

	virtual void deinit() = 0;

	virtual void reconfigure();

	virtual void select();

	virtual void unselect();

	virtual uint16_t transfer(uint16_t data) = 0;

	bool isInit() { return _init; };

protected:
	virtual bool initClock() = 0;
	SPI_TypeDef* _spi;
	GPIO_TypeDef* _csGPIO;
	uint16_t _csPin;
	unsigned char _bits;
	bool _cpol;
	bool _cpha;
	bool _init;
	bool _firstBitLSB;
	uint16_t _prescaler;

private:
};




#endif /* SPIBASE_HPP_ */
