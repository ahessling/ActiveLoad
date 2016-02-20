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
  if (crc16(0xFFFF, buf, serializedLength + CRC_SIZE) == 0)
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
  unsigned short crc = crc16(0xFFFF, buf, serializedLength);
  buf[serializedLength] = crc >> 8;
  buf[serializedLength + 1] = crc & 0xFF;

  return writeMemorySlot(memorySlot, buf, serializedLength + CRC_SIZE);
}

/** Write a new parameter buffer to a given memory slot.
 *
 * @param memorySlot Memory slot
 * @param buf Buffer of memory slot including CRC16
 * @param size Size of buf
 * @return
 */
int NVRAM::writeMemorySlot(enum MemoryLayout memorySlot, char* buf,
    unsigned int size)
{
  // write data

  // save whole parameter page

  // temporary buffer that holds all current parameters
  // this must be word-aligned and the size must be a multiple of 4!
  uint32_t savedParameters[(MemoryLayout::MemorySize + 3) >> 2];

  // read from flash
  memcpy(savedParameters, (char*)&_parameters_start, sizeof(savedParameters));

  // insert new serialized data into temporarily saved structure
  memcpy((char*)savedParameters + memorySlot, buf, size);

  // erase "old" parameters page
  eraseParameters();

  // write new parameters
  writeParametersToFlash(savedParameters, sizeof(savedParameters));

  return 0;
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
  char buf[32] = {0}; // should be big enough

  return writeMemorySlot(memorySlot, buf, size);
}

/** Erase the whole special parameters flash page.
 *
 */
void NVRAM::eraseParameters()
{
  FLASH_Unlock();

  /* Clear pending flags (if any) */
  FLASH_ClearFlag(FLASH_FLAG_EOP | FLASH_FLAG_PGERR | FLASH_FLAG_WRPERR);

  // erase whole page
  FLASH_ErasePage((uint32_t)&_parameters_start);

  FLASH_Lock();
}

/** Write the (new) parameters into special flash page.
 *
 * @param parametersBuf Word-aligned (!) buffer to new parameters
 * @param size Size of new parameters, must be a multiple of 4
 */
void NVRAM::writeParametersToFlash(uint32_t* parametersBuf, int size)
{
  FLASH_Unlock();

  /* Clear pending flags (if any) */
  FLASH_ClearFlag(FLASH_FLAG_EOP | FLASH_FLAG_PGERR | FLASH_FLAG_WRPERR);

  uint32_t flashAddress = (uint32_t)&_parameters_start;

  // write parameters
  while (size > 0)
  {
    // write 4 bytes
    if (FLASH_ProgramWord(flashAddress, *parametersBuf++) == FLASH_COMPLETE)
    {
      flashAddress += 4;
      size -= 4;
    }
    else
    {
      // flash error, should not happen
      break;
    }
  }

  FLASH_Lock();
}

