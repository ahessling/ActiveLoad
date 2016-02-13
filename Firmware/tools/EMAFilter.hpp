/**
 * @file EMAFilter.hpp
 *
 * @date 24.01.2016
 * @author Andre
 * @description
 */
#ifndef EMAFILTER_HPP_
#define EMAFILTER_HPP_

/** Exponential Moving Average (EMA) Filter
 *
 */
class EMAFilter
{
public:
  EMAFilter(unsigned int filterLength);
  virtual ~EMAFilter() { }

  void init();
  float execute(float newSample);

private:
  unsigned int _filterLength;
  float _filterOutput;
  float _lastFilterOutput;
  int _filterSamples;
};

#endif /* EMAFILTER_HPP_ */
