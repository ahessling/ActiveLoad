/**
 * @file DOGS104.hpp
 *
 * @date 13.02.2016
 * @author Andre
 * @description
 */
#ifndef DOGS104_HPP_
#define DOGS104_HPP_

#include <stdint.h>
#include <errno.h>

/** Electronic Assembly DOGS104 display base class.
 *
 * This class implements the high level functions.
 * The low level interface functions (SPI or I2C) need
 * to be implemented by a derivative class.
 */
class DOGS104
{
public:

  /** Font width */
  enum DogFontWidth
  {
    FONT_WIDTH_5 = 0, //!< 5-dot font width
    FONT_WIDTH_6      //!< 6-dot font width
  };

  /** Number of display lines */
  enum DogDisplayLines
  {
    LINES_1_2 = 0, //!< 1-line or 2-line display
    LINES_3_4      //!< 3-line or 4-line display
  };

  DOGS104(enum DogFontWidth fontWidth, enum DogDisplayLines lines,
      bool topView = false);
  virtual ~DOGS104() { }

  void init();

  void setContrast(unsigned char contrast);

  void clear();

  void write(unsigned char c);

  int write(unsigned char c, unsigned char x, unsigned char y);

  int gotoXY(unsigned char x, unsigned char y);

protected:
  virtual void reset() = 0;

  virtual int writeCommand(uint16_t command) = 0;

private:
  void setDDRAMAddress(unsigned char address);

  enum DogFontWidth _fontWidth;
  enum DogDisplayLines _lines;
  bool _topView;
};

#endif /* DOGS104_HPP_ */
