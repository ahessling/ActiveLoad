using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;

namespace ActiveLoadProtocol
{
    public class SerialPortBuffered
    {
        #region Private Members
        SerialPort serialPort;
        Queue<byte> queueIncoming = new Queue<byte>(16384);
        object lockIncoming = new object();
        #endregion

        #region Properties
        public int BaudRate
        {
            get
            {
                return serialPort.BaudRate;
            }
            set
            {
                serialPort.BaudRate = value;
            }
        }

        public string PortName
        {
            get
            {
                return serialPort.PortName;
            }
            set
            {
                serialPort.PortName = value;
            }
        }

        public Parity Parity
        {
            get
            {
                return serialPort.Parity;
            }
            set
            {
                serialPort.Parity = value;
            }
        }

        public int DataBits
        {
            get
            {
                return serialPort.DataBits;
            }
            set
            {
                serialPort.DataBits = value;
            }
        }

        public StopBits StopBits
        {
            get
            {
                return serialPort.StopBits;
            }
            set
            {
                serialPort.StopBits = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return serialPort.IsOpen;
            }
        }

        public Handshake Handshake
        {
            get
            {
                return serialPort.Handshake;
            }
            set
            {
                serialPort.Handshake = value;
            }
        }

        public int BytesInQueue
        {
            get
            {
                return queueIncoming.Count;
            }
        }
        #endregion

        #region Events
        public delegate void DataReceived();
        private DataReceived dataReceivedHandler;

        public delegate void SerialError(Exception exc);
        public event SerialError SerialErrorEvent;

        public event DataReceived DataReceivedEvent
        {
            add
            {
                dataReceivedHandler += value;

                // Raise event immediately if there is something in the incoming queue
                lock (lockIncoming)
                {
                    if (queueIncoming.Count > 0)
                    {
                        dataReceivedHandler();
                    }
                }
            }

            remove
            {
                dataReceivedHandler -= value;
            }
        }

        void raiseDataReceived()
        {
            // Raise event if event handler is available
            if (dataReceivedHandler != null)
            {
                dataReceivedHandler();
            }
        }

        void raiseSerialError(Exception exc)
        {
            if (SerialErrorEvent != null)
            {
                SerialErrorEvent(exc);
            }
        }
        #endregion

        #region Constructors
        public SerialPortBuffered(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        }

        public SerialPortBuffered(string portName)
        {
            serialPort = new SerialPort(portName);
        }

        public SerialPortBuffered(SerialPort serialPort)
        {
            this.serialPort = serialPort;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Write a string to the serial port.
        /// </summary>
        /// <param name="text">String to send</param>
        public void Write(string text)
        {
            serialPort.Write(text);
        }

        /// <summary>
        /// Write a line (ending with CR+LF) to the serial port.
        /// </summary>
        /// <param name="text">String to send</param>
        public void WriteLine(string text)
        {
            serialPort.WriteLine(text);
        }

        /// <summary>
        /// Write a byte array to the serial port.
        /// </summary>
        /// <param name="buffer">Byte array</param>
        /// <param name="offset">Offset in array</param>
        /// <param name="count">Number of bytes to write</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            serialPort.Write(buffer, offset, count);
        }

        /// <summary>
        /// Write a character array to the serial port.
        /// </summary>
        /// <param name="buffer">Char array</param>
        /// <param name="offset">Offset in array</param>
        /// <param name="count">Number of bytes to write</param>
        public void Write(char[] buffer, int offset, int count)
        {
            serialPort.Write(buffer, offset, count);
        }

        /// <summary>
        /// Open the serial port and start receiving data asynchronously.
        /// </summary>
        public async void Open()
        {
            serialPort.Open();

            if (serialPort.IsOpen)
            {
                // Start reading asynchronously the safe way
                // taken from: http://www.sparxeng.com/blog/software/must-use-net-system-io-ports-serialport

                byte[] buffer = new byte[1024];

                try
                {
                    while (serialPort.IsOpen)
                    {
                        // Async wait for data
                        int actualLength = await serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length);

                        // Copy actual received data and raise event
                        byte[] received = new byte[actualLength];
                        Buffer.BlockCopy(buffer, 0, received, 0, actualLength);

                        // Enqueue incoming elements for later retrieval
                        lock (lockIncoming)
                        {
                            foreach (byte b in received)
                            {
                                queueIncoming.Enqueue(b);
                            }
                        }

                        // Notify application
                        raiseDataReceived();
                    }
                }
                catch (Exception exc)
                {
                    raiseSerialError(exc);
                }
            }
        }

        /// <summary>
        /// Close the serial port.
        /// </summary>
        public void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        /// <summary>
        /// Clear the incoming FIFO queue.
        /// </summary>
        public void ClearIncoming()
        {
            lock (lockIncoming)
            {
                queueIncoming.Clear();
            }
        }

        /// <summary>
        /// Return all elements in incoming receive FIFO queue.
        /// </summary>
        /// <returns>Array with incoming bytes</returns>
        public byte[] ReadAll()
        {
            lock (lockIncoming)
            {
                // Dequeue all elements and return a copy of the buffer
                byte[] bytesIncoming = new byte[queueIncoming.Count];

                for (int i = 0; i < bytesIncoming.Length; i++)
                {
                    bytesIncoming[i] = queueIncoming.Dequeue();
                }

                return bytesIncoming;
            }
        }

        /// <summary>
        /// Read one byte from incoming FIFO queue.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the queue is empty</exception>
        /// <returns>Oldest byte in queue</returns>
        public byte Read()
        {
            lock (lockIncoming)
            {
                return queueIncoming.Dequeue();
            }
        }
        #endregion
    }
}
