/**
 * @file Serializable.hpp
 *
 * @date 26.01.2016
 * @author Andre
 * @description
 */
#ifndef SERIALIZABLE_HPP_
#define SERIALIZABLE_HPP_

class Serializable
{
public:
  Serializable()
  {
    _serializedDataLength = 0;
  }

  virtual ~Serializable() { }

  /** Returns the number of bytes needed to serialize the class.
   *
   * @return Number of bytes needed to serialize
   */
  virtual unsigned int getSerializedDataLength() const
  {
    return _serializedDataLength;
  }

  /** Serialize internal data structures to a byte array.
   *
   * @param serialBuf Array which will contain the serialized structure
   * @param serialBufLength Number of bytes in array
   * @return error code
   */
  virtual int serialize(char* serialBuf, unsigned int serialBufLength) const
  {
    if (!serialBuf)
      return -1;

    // not enough space in array
    if (serialBufLength < _serializedDataLength)
      return -1;

    return _serialize(serialBuf, serialBufLength);
  }

  /** Deserialize from a byte array to internal data structures.
   *
   * @param serialBuf Array with serial data
   * @param serialBufLength Number of bytes in array
   * @return error code
   */
  virtual int deserialize(const char* serialBuf, unsigned int serialBufLength)
  {
    if (!serialBuf)
      return -1;

    // incomplete deserialization
    if (serialBufLength < _serializedDataLength)
      return -1;

    return _deserialize(serialBuf, serialBufLength);
  }

protected:
  virtual int _serialize(char* serialBuf, unsigned int serialBufLength) const = 0;

  virtual int _deserialize(const char* serialBuf, unsigned int serialBufLength) = 0;

  unsigned int _serializedDataLength; ///< Number of bytes used in serialized state
};

#endif /* SERIALIZABLE_HPP_ */
