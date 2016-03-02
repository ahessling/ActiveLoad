/**
 * @file DOGS104.cpp
 *
 * @date 13.02.2016
 * @author Andre
 * @description
 */
#include "DOGS104.hpp"
#include <string.h>

#define MAX_LINES       4  // Max. number of lines
#define MAX_X           10 // Max. characters per line
#define MAX_ADDRESS     (MAX_LINES * MAX_X - 1) // Highest possible (logical) address

DOGS104::DOGS104(enum DogFontWidth fontWidth, enum DogDisplayLines lines,
    bool topView) :
    _fontWidth(fontWidth),
    _lines(lines),
    _topView(topView)
{
  _doubleBuffered = false;
  _cursorPos = 0;
  _dirtyFrom = 0;
  _dirtyTo = 0;
  _dirty = false;
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

  _cursorPos = 0;
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

  if (true == _doubleBuffered)
  {
    memset(_frameBuffer, 0, sizeof(_frameBuffer));
    _dirtyFrom = 0;
    _dirtyTo = MAX_ADDRESS;
  }
}

/** Go to a specified X/Y position.
 *
 * @param x X position (character position in line from 0..9)
 * @param y Y position (line from 0..3)
 * @return 0
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

  // remember cursor position
  _cursorPos = x + y * MAX_X;

  // if display is double buffered, there is nothing else to do here

  if (false == _doubleBuffered)
  {
    // not double buffered, so set DDRAM address
    return _gotoXY(x, y);
  }

  return 0;
}

int DOGS104::_gotoXY(unsigned char x, unsigned char y)
{
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
  if (false == _doubleBuffered)
  {
    // write directly to LCD
    writeCommand(0x200 | c);
  }
  else
  {
    // use framebuffer, no immediate update

    if (false == _dirty)
    {
      // save first dirty position
      _dirtyFrom = _cursorPos;
    }

    // set dirty flags and calculate dirty positions
    _dirty = true;

    if (_cursorPos < _dirtyFrom)
    {
      _dirtyFrom = _cursorPos;
    }

    _frameBuffer[_cursorPos++] = c;

    if (_cursorPos > _dirtyTo)
    {
      _dirtyTo = _cursorPos;
    }

    // wrap around
    if (_cursorPos > MAX_ADDRESS)
    {
      _cursorPos = 0;
    }
  }
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

/** Enable/disable LCD double buffering using an internal framebuffer.
 *
 * When the framebuffer is initially enabled, the framebuffer will be cleared.
 *
 * @param doubleBuffered True/false
 */
void DOGS104::setDoubleBuffered(bool doubleBuffered)
{
  // clear framebuffer
  if (true == doubleBuffered && _doubleBuffered != doubleBuffered)
  {
    memset(_frameBuffer, 0, sizeof(_frameBuffer));
    _dirtyFrom = 0;
    _dirtyTo = MAX_ADDRESS;
    _dirty = true;
  }

  _doubleBuffered = doubleBuffered;
}

/** Refresh the display when the LCD is double buffered.
 *
 * Only the modified characters are written, the rest is untouched.
 *
 * @note Does nothing if display is not double buffered.
 */
void DOGS104::refresh()
{
  if (true == _doubleBuffered)
  {
    unsigned char x = _dirtyFrom % MAX_X;
    unsigned char y = _dirtyFrom / MAX_X;

    // go to position of first changed data
    gotoXY(x, y);

    // only write "dirty" characters to LCD
    while (_dirtyFrom != _dirtyTo)
    {
      // write directly to LCD
      writeCommand(0x200 | _frameBuffer[_dirtyFrom++]);

      // move cursor to new line if line end is reached
      if ((_dirtyFrom % MAX_X) == 0)
      {
        gotoXY(0, ++y);
      }
    }

    // reset dirty flags
    _dirtyFrom = 0;
    _dirtyTo = 0;
    _dirty = false;
  }
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
