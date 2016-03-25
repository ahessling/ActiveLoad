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
#include "driver/DOGS104_SPI.hpp"

class HMI_Front : public HMI
{
public:
  HMI_Front(SPIBase* spi);

  virtual ~HMI_Front() { }

  virtual bool execute(SystemState& systemState, SystemCommand& systemCommand);

private:
  void lowLevelInit();
  void updateDisplay(const SystemState& systemState);
  void updateLED(const SystemState& systemState);

  SPIBase* _spi;
  uint16_t _encoderCounter;
  uint16_t _lastEncoderCounter;
  DOGS104_SPI _display;
  uint32_t _lastDisplayUpdate;
  unsigned int _blinkTimer;    ///< Blink timer
  bool _displayBlinkState;     ///< Display blink state
  bool _ledBlinkState;         ///< LED blink state
  bool _oldKeyPress;
  float _oldSetpointCurrent;
};

#endif /* HMI_FRONT_HPP_ */
