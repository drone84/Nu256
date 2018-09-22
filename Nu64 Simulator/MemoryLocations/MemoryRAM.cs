using Nu64.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    public class MemoryRAM : IMappable
    {
        // allocate 16MB of memory
        protected byte[] data = null;
        private int startAddress;

        public int StartAddress
        {
            get
            {
                return this.startAddress;
            }
        }

        public int Length
        {
            get
            {
                return data.Length;
            }
        }

        public MemoryRAM(int StartAddress, int Size)
        {
            this.startAddress = StartAddress;
            data = new byte[Size];
        }

        public virtual byte ReadByte(int Address)
        {
            return data[Address];
        }

        public virtual void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
        }

        internal void Load(byte[] SourceData, int SrcStart, int DestStart, int length)
        {
            for (int i = 0; i < length; i++)
            {
                this.data[DestStart + i] = SourceData[SrcStart + i];
            }
        }

        /// <summary>
        /// Reads a 16-bit word from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public int ReadWord(int Address)
        {
            return ReadByte(Address) + (ReadByte(Address+1) << 8);
        }

        public void WriteWord(int Address, int Value)
        {
            WriteByte(Address, (byte)(Value & 0xff));
            WriteByte(Address + 1, (byte)(Value >> 8 & 0xff));
        }

        internal int ReadLong(int Address)
        {
            return ReadByte(Address) + (ReadByte(Address + 1) << 8) + (ReadByte(Address + 1) << 16); 
        }

        internal void Copy(int SourceAddress, MemoryRAM Destination, int DestAddress, int Length)
        {
            for(int i=0; i<Length; ++i)
            {
                Destination.data[DestAddress + i] = data[SourceAddress + i];
            }
        }
    }
}
