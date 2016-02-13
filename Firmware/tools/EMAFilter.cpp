/**
 * @file EMAFilter.cpp
 *
 * @date 24.01.2016
 * @author Andre
 * @description
 */
#include "EMAFilter.hpp"

/** Implements an exponential moving average (EMA) filter.
 *
 * The filter formula is: y[n] = y[n-1] + (x[n] - y[n-1]) / N
 *
 * N: filter length
 *
 * Call execute when there is a new sample that should be put
 * into the filter.
 *
 * @param filterLength Number of datum points
 */
EMAFilter::EMAFilter(unsigned int filterLength)
{
  _filterLength = filterLength;

  init();
}

/** Resets the filter. */
void EMAFilter::init()
{
  _lastFilterOutput = 0.0f;
  _filterSamples = 0;
  _filterOutput = 0.0f;
}

/** Calculates the next filter response.
 *
 * @param newSample New input sample
 * @return New filter output
 */
float EMAFilter::execute(float newSample)
{
  if (_filterLength > 0)
  {
    if (0 == _filterSamples)
    {
      // init filter at first input
      _filterOutput = newSample;
      _filterSamples++;
    }
    else
    {
      // filter already initialized

      // y[n] = y[n-1] + (x[n] - y[n-1]) / N
      _filterOutput = _lastFilterOutput + (newSample - _lastFilterOutput) / (float)_filterLength;
    }

    _lastFilterOutput = _filterOutput;
  }
  else
  {
    // Bypass filter
    _filterOutput = newSample;
  }

  return _filterOutput;
}
