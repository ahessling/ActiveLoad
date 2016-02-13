/**
 * @file crc16.h
 *
 * @date 26.01.2016
 * @author Andre
 * @description
 */
#ifndef CRC16_H_
#define CRC16_H_

#ifdef __cplusplus
 extern "C" {
#endif

 /** Do a CRC-16 block calculation based on a lookup table.
  *
  * \note Be aware of the "CRC null problem". This can be avoided if
  * the initial CRC value is set to a value other than 0 (e.g. 0xFF).
  *
  * @param crc Initial CRC value, recommendation: 0xFF
  * @param buf Data buffer
  * @param len Number of bytes in buf
  */
unsigned short crc16(unsigned short crc, const char *buf, int len);

#ifdef __cplusplus
}
#endif


#endif /* CRC16_H_ */
