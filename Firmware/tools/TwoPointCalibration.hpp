/**
 * @file TwoPointCalibration.hpp
 *
 * @date 27.01.2016
 * @author Andre
 * @description
 */
#ifndef TWOPOINTCALIBRATION_HPP_
#define TWOPOINTCALIBRATION_HPP_

#include "Serializable.hpp"

class TwoPointCalibration : public Serializable
{
public:
  TwoPointCalibration();
  TwoPointCalibration(float gain, float offset);

  virtual ~TwoPointCalibration() { }

  void calibrate(float x[2], float y[2]);

  void clearCalibrationData();

  float translate(float input) const;

  bool isCalibrated() const
  {
    return _calibrated;
  }

  static const int serializedDataLength;

private:
  virtual int _serialize(char* serialBuf, unsigned int serialBufLength) const;

  virtual int _deserialize(const char* serialBuf, unsigned int serialBufLength);

  float _gain;
  float _offset;
  bool _calibrated;
};

#endif /* TWOPOINTCALIBRATION_HPP_ */
