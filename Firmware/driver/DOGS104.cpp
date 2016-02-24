/**
 * @file DOGS104.cpp
 *
 * @date 13.02.2016
 * @author Andre
 * @description
 */
#include "DOGS104.hpp"

#define MAX_LINES   4  // Max. number of lines
#define MAX_X       10 // Max. characters per line

DOGS104::DOGS104(enum DogFontWidth fontWidth, enum DogDisplayLines lines,
    bool topView) :
    _fontWidth(fontWidth),
    _lines(lines),
    _topView(topView)
{
}

/** Initializes the display and turns it on.
 *
 */
void DOGS104::init()
{
  // reset the display
  reset();

  // initialize the display and turn it on
  writeCommand(0x3A); // 8 bit data length extension (RE=1, REV=0)

  // set font width and number of lines
  // fixme: needs work and testing
  unsigned char fontConfig = 0x08;

  if (FONT_WIDTH_6 == _fontWidth)
  {
    fontConfig |= 0x04;
  }

  if (LINES_3_4 == _lines)
  {
    fontConfig |= 0x01;
  }

  writeCommand(fontConfig);

  // orientation
  if (false == _topView)
  {
    // bottom view
    writeCommand(0x06);
  }
  else
  {
    // top view
    writeCommand(0x05);
  }

  writeCommand(0x1E); // Bias setting BS1=1
  writeCommand(0x39); // 8 bit data length extension (RE=0, IS=1)
  writeCommand(0x1B); // BS0=1 --> Bias=1/6
  writeCommand(0x6E); // Divider on and set value
  writeCommand(0x56); // Booster on and set contrast (DB1=C5, DB0=C4)
  writeCommand(0x7A); // Set contrast (DB3-DB0=C3-C0)
  writeCommand(0x38); // 8 bit data length extension (RE=0, IS=0)
  writeCommand(0x08); // Display on
}

/** Set the display contrast.
 *
 * @param contrast Contrast setting between 0 and 63
 */
void DOGS104::setContrast(unsigned char contrast)
{
  contrast &= 0x3F;

  writeCommand(0x39); // 8 bit data length extension (RE=0, IS=1)
  writeCommand(0x50 | (contrast >> 4)); // Booster on and set contrast (DB1=C5, DB0=C4)
  writeCommand(0x70 | (contrast & 0x0F)); // Set contrast (DB3-DB0=C3-C0)
  writeCommand(0x38); // 8 bit data length extension (RE=0, IS=0)
}

/** Clear the whole display.
 *
 */
void DOGS104::clear()
{
  writeCommand(0x01);
}

/** Go to a specified X/Y position.
 *
 * @param x X position (character position in line from 0..9)
 * @param y Y position (line from 0..3)
 * @return
 */
int DOGS104::gotoXY(unsigned char x, unsigned char y)
{
  if (x >= MAX_X)
  {
    return -EINVAL;
  }

  if (y >= MAX_LINES)
  {
    return -EINVAL;
  }

  // calculate DDRAM address
  unsigned char address;

  // each line beginning is 0x20 apart
  address = y * 0x20 + x;

  // top view orientation has a 0x0A offset
  if (true == _topView)
  {
    address += 0x0A;
  }

  setDDRAMAddress(address);

  return 0;
}

/** Write a character to the current position.
 *
 * @param c Character to write
 */
void DOGS104::write(unsigned char c)
{
  writeCommand(0x200 | c);
}

/** Write a character to a specified position.
 *
 * @param c Character
 * @param x X position
 * @param y Y position
 * @return 0, otherwise -EINVAL if position is invalid
 */
int DOGS104::write(unsigned char c, unsigned char x, unsigned char y)
{
  int ret = gotoXY(x, y);

  if (0 == ret)
  {
    write(c);
    return 0;
  }
  else
  {
    return ret;
  }
}

/** Write a null-terminated string to the current position.
 *
 * @param str Null-terminated string
 * @return 0
 */
int DOGS104::write(char* str)
{
  if (0 != str)
  {
    while (*str)
    {
      write(*str++);
    }
  }

  return 0;
}

/** Write a null-terminated string to a specified position.
 *
 * @param str Null-terminated string
 * @param x X position
 * @param y Y position
 * @return 0, otherwise -EINVAL if position is invalid
 */
int DOGS104::write(char* str, unsigned char x, unsigned char y)
{
  int ret = gotoXY(x, y);

  if (0 == ret)
  {
    ret = write(str);
  }

  return ret;
}

/** Set the DDRAM address.
 *
 * @param address DDRAM address (0..127)
 */
void DOGS104::setDDRAMAddress(unsigned char address)
{
  address &= 0x7F;

  writeCommand(0x80 | address);
}
