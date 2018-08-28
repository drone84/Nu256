using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    // Buffer
    //
    // Holds data for input/output
    // The Read and Write addresses determine whether data is available.
    //
    // Data is stored at WritePos and read from ReadPos. 
    // WritePos always points to the next byte to write. 
    // ReadPos always points to the next byte to be read. 
    //
    // When ReadPos = WritePos, the buffer is empty
    // When WritePos = ReadPos - 1, the buffer is full
    // 

    public class MemoryBuffer
    {
        public MemoryRAM Memory = null;
        int StartAddress;
        int EndAddress;
        int ReadPosAddress = 0;
        int WritePosAddress = 0;


        public MemoryBuffer(MemoryRAM newMemory, int newStartAddress, int newEndAddress, int newReadPosAddress, int newWritePosAddress)
        {
            this.Memory = newMemory;
            this.StartAddress = newStartAddress;
            this.EndAddress = newEndAddress;
            this.ReadPosAddress = newReadPosAddress;
            this.WritePosAddress = newWritePosAddress;

            WritePos = newStartAddress;
            ReadPos = newStartAddress;
        }

        public void Write(int Value)
        {
            Write(Value, 2);
        }

        public void Write(byte Value)
        {
            Write(Value, 1);
        }

        public void Write(int Value, int Bytes)
        {
            if (Count >= Capacity)
                return;

            if (Bytes == 1)
                Memory.WriteByte(WritePos, (byte)Value);
            if (Bytes == 2)
                Memory.WriteWord(WritePos, Value);

            WritePos = WritePos + Bytes;
            if (WritePos > EndAddress)
                WritePos = StartAddress;
        }

        public int Read(int Bytes)
        {
            int ret = 0;
            // buffer is empty. 
            if (Count == 0)
                return 0;

            if (Bytes == 1)
                ret = Memory.ReadByte(ReadPos);
            if (Bytes == 2)
                ret = Memory.ReadWord(ReadPos);

            ReadPos = ReadPos + Bytes;
            if (ReadPos > EndAddress)
                ReadPos = StartAddress;

            return ret;
        }

        public bool IsEmpty()
        {
            return ReadPos == WritePos;
        }

        public int Count
        {
            get
            {
                int len = WritePos - ReadPos;
                while (len < 0)
                    len += Capacity;
                return len;
            }
        }

        public int Capacity
        {
            get
            {
                return EndAddress - StartAddress;
            }
        }

        public int ReadPos
        {
            get
            {
                return Memory.ReadWord(ReadPosAddress);
            }

            protected set
            {
                Memory.WriteWord(ReadPosAddress, value);
            }
        }

        public int WritePos
        {
            get
            {
                return Memory.ReadWord(WritePosAddress);
            }

            protected set
            {
                Memory.WriteWord(WritePosAddress, value);
            }
        }

        public void Clear()
        {
            ReadPos = WritePos;
        }

        public int Peek(int Bytes)
        {
            int ret = 0;
            // buffer is empty. 
            if (Count == 0)
                return 0;

            if (Bytes == 1)
                ret = Memory.ReadByte(ReadPos);
            if (Bytes == 2)
                ret = Memory.ReadWord(ReadPos);

            return ret;
        }

        public void Discard(int Num)
        {
            while (Num > 0)
            {
                ReadPos++;
                if (ReadPos > EndAddress)
                    ReadPos = StartAddress;
            }
        }
    }
}
