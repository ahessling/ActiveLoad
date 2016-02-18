/**
 * @file NVRAM.cpp
 *
 * @date 27.01.2016
 * @author Andre
 * @description
 */
#include "NVRAM.hpp"
#include "tools/crc16.h"
#include "hw_config.h"
#include <string.h>

#define CRC_SIZE    2 // 2 bytes CRC

extern uint32_t _parameters_start; ///< Start address of parameters in flash section

NVRAM::NVRAM()
{
  lowLevelInit();
}

void NVRAM::lowLevelInit()
{
}

/** Load a serializable class from a memory slot.
 *
 * @param memorySlot Memory Slot
 * @param memoryData Serializable class which will contain the data of the memory slot
 * @return 0 if OK, otherwise error code
 */
int NVRAM::loadMemorySlot(enum MemoryLayout memorySlot, Serializable& memoryData)
{
  // read data set
  char buf[32]; // should be big enough

  unsigned int serializedLength = memoryData.getSerializedDataLength();

  // read from flash
  memcpy(buf, (char*)&_parameters_start + memorySlot, serializedLength + CRC_SIZE);

  int result = 0;

  // validate checksum
  if (crc16(0xFF, buf, serializedLength + CRC_SIZE) == 0)
  {
    // deserialize NVRAM data to concrete object
    return memoryData.deserialize(buf, sizeof(buf));
  }
  else
  {
    result = -EBADCHECKSUM;
  }

  return result;
}

/** Save a serializable class into a memory slot.
 *
 * @param memorySlot Memory slot
 * @param memoryData Serializable class which will be saved
 * @return 0 if OK, otherwise error code
 */
int NVRAM::saveMemorySlot(enum MemoryLayout memorySlot,
    const Serializable& memoryData)
{
  // serialize class
  char buf[32]; // should be big enough
  memoryData.serialize(buf, sizeof(buf));

  // calculate CRC
  unsigned int serializedLength = memoryData.getSerializedDataLength();
  unsigned short crc = crc16(0xFF, buf, serializedLength);
  buf[serializedLength] = crc >> 8;
  buf[serializedLength + 1] = crc & 0xFF;

  // write data
  //return _fram.writeBytes(memorySlot, buf, serializedLength + CRC_SIZE);
  return 0;

  // todo: implement
}

/** Clear a memory slot by writing zeroes to it, thus invalidating all data and
 * the CRC checksum.
 *
 * @param memorySlot Memory slot
 * @param size Size of memory slot
 * @return 0 if OK, otherwise error code
 */
int NVRAM::clearMemorySlot(enum MemoryLayout memorySlot, unsigned int size)
{
  int ret = 0;

  unsigned int address = memorySlot;

  // clear by writing zeroes
  while (size-- && 0 == ret)
  {
    //ret = _fram.writeByte(address++, 0);
    // todo: implement
  }

  return ret;
}
