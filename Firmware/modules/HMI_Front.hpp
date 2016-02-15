/**
 * @file HMI_Front.hpp
 *
 * @date 13.02.2016
 * @author Andre
 * @description
 */
#ifndef HMI_FRONT_HPP_
#define HMI_FRONT_HPP_

#include <stdint.h>
#include "HMI.hpp"
#include "driver/spibase.hpp"

class HMI_Front : public HMI
{
public:
  HMI_Front(SPIBase* spi);

  virtual ~HMI_Front() { }

  virtual bool execute(SystemState& systemState, SystemCommand& systemCommand);

private:
  void lowLevelInit();

  SPIBase* _spi;
  uint16_t _encoderCounter;
  uint16_t _lastEncoderCounter;
};

#endif /* HMI_FRONT_HPP_ */
