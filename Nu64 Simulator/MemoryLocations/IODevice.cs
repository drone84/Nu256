using Nu64.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    /// <summary>
    ///  base class for I/O device. 
    /// </summary>
    public class IODevice : IMappable
    {
        public byte ReadByte(int Address)
        {
            throw new NotImplementedException("IODevice: Cannot read address " + Address.ToString("X4"));

        }

        public void WriteByte(int Address, byte Data)
        {
            throw new NotImplementedException("IODevice: Cannot write address " + Address.ToString("X4"));
        }
    }
}
