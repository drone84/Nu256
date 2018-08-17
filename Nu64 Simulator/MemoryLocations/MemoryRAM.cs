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

        public MemoryRAM(int Size)
        {
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
    }
}
