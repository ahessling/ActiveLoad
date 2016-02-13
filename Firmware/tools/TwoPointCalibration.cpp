/**
 * @file TwoPointCalibration.cpp
 *
 * @date 27.01.2016
 * @author Andre
 * @description
 */
#include "TwoPointCalibration.hpp"
#include <string.h>

const int TwoPointCalibration::serializedDataLength = 8; ///< Number of bytes used for serialization

/** Initialize with unity gain and null offset.
 *
 */
TwoPointCalibration::TwoPointCalibration() : TwoPointCalibration(1.0f, 0.0f)
{
  _calibrated = false;
}

/** Initialize with known gain and offset.
 *
 * @param gain Gain
 * @param offset Offset
 */
TwoPointCalibration::TwoPointCalibration(float gain, float offset)
{
  _gain = gain;
  _offset = offset;
  _calibrated = true;

  _serializedDataLength = TwoPointCalibration::serializedDataLength;
}

/** Calculate gain and offset based on two inputs and (measured) outputs.
 *
 * The assumed formula is: y = gain * x + offset
 *
 * @param x Input vector (two points)
 * @param y Output vector (two points)
 */
void TwoPointCalibration::calibrate(float x[2], float y[2])
{
  // assume: y = gain * x + offset

  // calculate gain and offset
  _gain = (y[0] - y[1]) / (x[0] - x[1]);
  _offset = y[0] - _gain * x[0];

  _calibrated = true;
}

/** Calculate output based on gain and offset.
 *
 * @param input Input
 * @return Output
 */
float TwoPointCalibration::translate(float input) const
{
  return (input - _offset) / _gain;
}

int TwoPointCalibration::_serialize(char* serialBuf,
    unsigned int serialBufLength) const
{
  (void)serialBufLength;
  memcpy(serialBuf, &_gain, sizeof(_gain));
  serialBuf += sizeof(_gain);
  memcpy(serialBuf, &_offset, sizeof(_offset));

  return 0;
}

/** Reset calibration data.
 *
 * Gain = 1, Offset = 0
 */
void TwoPointCalibration::clearCalibrationData()
{
  _gain = 1.0f;
  _offset = 0.0f;
  _calibrated = false;
}

int TwoPointCalibration::_deserialize(const char* serialBuf,
    unsigned int serialBufLength)
{
  (void)serialBufLength;
  memcpy(&_gain, serialBuf, sizeof(_gain));
  serialBuf += sizeof(_gain);
  memcpy(&_offset, serialBuf, sizeof(_offset));

  _calibrated = true;

  return 0;
}
