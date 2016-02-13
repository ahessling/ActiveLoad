/**
 * @file spibase.cpp
 *
 * @date 24.01.2015
 * @author Andre
 * @description
 */

#include "spibase.hpp"

SPIBase::SPIBase(SPI_TypeDef *spi)
{
	_spi = spi;
	_init = false;
	_csGPIO = 0;
	_bits = 8;
	_prescaler = 0;
	_cpol = false;
	_cpha = false;
	_firstBitLSB = false;
	_csPin = 0;
}

SPIBase::~SPIBase()
{

}

void SPIBase::select()
{
	if (_csGPIO != 0)
		_csGPIO->BRR = _csPin;
}

void SPIBase::unselect()
{
	if (_csGPIO != 0)
		_csGPIO->BSRR = _csPin;
}

void SPIBase::reconfigure()
{

}
