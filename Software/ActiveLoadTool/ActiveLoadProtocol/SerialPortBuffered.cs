using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ActiveLoadProtocol
{
    public static class TaskExt
    {
        /// <summary>
        /// Taken from: http://blogs.msdn.com/b/pfxteam/archive/2012/10/05/how-do-i-cancel-non-cancelable-async-operations.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> WithCancellation<T>(
            this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(
                        s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            return await task;
        }
    }

    public class SerialPortBuffered : IASCIIReadWrite
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

        #region Wrappers
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
        /// Open the serial port.
        /// </summary>
        public void Open()
        {
            serialPort.Open();
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

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return await serialPort.BaseStream.ReadAsync(buffer, offset, count, cancellationToken);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Clear the incoming FIFO queue.
        /// </summary>
        public void ClearIncoming()
        {
            lock (lockIncoming)
            {
                queueIncoming.Clear();
            }

            // Flush the internal SerialPort buffer
            serialPort.BaseStream.Flush();
        }

        /// <summary>
        /// Return all elements in incoming receive FIFO queue.
        /// </summary>
        /// <returns>Array with incoming bytes</returns>
        public byte[] ReadExisting()
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
        public byte ReadByte()
        {
            lock (lockIncoming)
            {
                return queueIncoming.Dequeue();
            }
        }

        public async Task<string> ReadUntilAsync(string delimiter, int timeout)
        {
            CancellationTokenSource readCancellationTokenSource = new CancellationTokenSource(timeout);

            Func<Task<string>> funcReceiveTask = async () =>
            {
                string receivedString = "";

                byte[] buffer = new byte[128];

                try
                {
                    while (!receivedString.Contains(delimiter))
                    {
                        // Async wait for data
                        Task<int> op = serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length);

                        int actualLength = await op.WithCancellation(readCancellationTokenSource.Token);

                        // Copy actual received data and concat string
                        byte[] received = new byte[actualLength];
                        Buffer.BlockCopy(buffer, 0, received, 0, actualLength);

                        receivedString += ASCIIEncoding.ASCII.GetString(received);
                    }

                    return receivedString.Substring(0, receivedString.IndexOf(delimiter));
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("ReadUntil timeout");

                    throw new TimeoutException();
                }
            };

            return await funcReceiveTask();
        }

        public string Read(int timeout)
        {
            // todo
            return "";
        }

        public void FlushIncoming()
        {
            ClearIncoming();
        }
        #endregion
    }
}
