using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu64.Processor;
using Nu64.Display;
using Nu64.MemoryLocations;

namespace Nu64
{
    /// <summary>
    /// Maps an address on the bus to a device or memory. GPU, RAM, and ROM are hard coded. Other I/O devices will be added 
    /// later.
    /// </summary>
    public class AddressDataBus : Nu64.Common.IMappable
    {
        public const int MinAddress = 0x000000;
        public const int MaxAddress = 0xffffff;

        public Gpu GPU = null;
        public IODevice GenericDevice = new IODevice();
        public MemoryRAM ROM = null;
        public MemoryRAM RAM = null;

        public SortedList<int, IODevice> IODevices = new SortedList<int, IODevice>();


        /// <summary>
        /// Determine whehter the address being read from or written to is an I/O device or a memory cell.
        /// If the location is an I/O device, return that device. Otherwise, return the memory being referenced.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Device"></param>
        /// <param name="Offset"></param>
        public void GetDeviceAt(int Address, out Nu64.Common.IMappable Device, out int Offset)
        {
            if (Address >= MemoryMap_Blocks.START_OF_DIRECT_PAGE && Address < MemoryMap_Blocks.START_OF_IO)
            {
                Device = RAM;
                Offset = MemoryLocations.MemoryMap_Blocks.START_OF_DIRECT_PAGE;
                return;
            }

            if (Address > MemoryMap_Blocks.END_OF_IO && Address <= MemoryMap_Blocks.END_OF_DIRECT_PAGE)
            {
                Device = RAM;
                Offset = MemoryLocations.MemoryMap_Blocks.START_OF_DIRECT_PAGE;
                return;
            }

            if (Address > MemoryMap_Blocks.END_OF_DIRECT_PAGE && Address <= MemoryMap_Blocks.END_OF_RAM)
            {
                Device = RAM;
                Offset = MemoryLocations.MemoryMap_Blocks.START_OF_RAM;
                return;
            }

            if (Address >= MemoryLocations.MemoryMap_Blocks.START_OF_ROM && Address <= MemoryLocations.MemoryMap_Blocks.END_OF_ROM)
            {
                Device = ROM;
                Offset = MemoryLocations.MemoryMap_Blocks.START_OF_ROM;
                return;
            }

            if (Address >= MemoryLocations.MemoryMap_Blocks.START_OF_GPU && Address <= MemoryLocations.MemoryMap_Blocks.END_OF_GPU)
            {
                Device = GPU;
                Offset = MemoryLocations.MemoryMap_Blocks.START_OF_GPU;
                return;
            }

            if (Address >= MemoryMap_Blocks.START_OF_IO && Address <= MemoryMap_Blocks.END_OF_IO)
            {
                Device = GenericDevice;
                Offset = MemoryLocations.MemoryMap_Blocks.START_OF_IO;
                return;
            }

            // oops, we didn't map this to anything. 
            Device = null;
            Offset = 0;
        }

        public virtual byte this[int Address]
        {
            get { return ReadByte(Address); }
            set { WriteByte(Address, value); ; }
        }

        public virtual byte this[int Bank, int Address]
        {
            get { return ReadByte(Bank * 0xffff + Address & 0xffff); }
            set { WriteByte(Bank * 0xffff + Address & 0xffff, value); }
        }

        public virtual byte ReadByte(int Address)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            return device.ReadByte(Address - offset);
        }

        /// <summary>
        /// Reads a 16-bit word from memory
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public int ReadWord(int Address)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            return device.ReadByte(Address - offset) + (device.ReadByte(Address - offset + 1) << 8);
        }

        /// <summary>
        /// Reads 3 bytes from memory and builds a 24-bit unsigned integer.
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public int ReadLong(int Address)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            return device.ReadByte(Address - offset)
                + (device.ReadByte(Address - offset + 1) << 8)
                + (device.ReadByte(Address - offset + 2) << 16);
        }

        public virtual void WriteByte(int Address, byte Value)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            device.WriteByte(Address - offset, Value);
        }

        public void WriteWord(int Address, int Value)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            device.WriteByte(Address, (byte)(Value & 0xff));
            device.WriteByte(Address + 1, (byte)(Value >> 8 & 0xff));
        }

        public void WriteLong(int Address, int Value)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            device.WriteByte(Address - offset, (byte)(Value & 0xff));
            device.WriteByte(Address - offset + 1, (byte)(Value >> 8 & 0xff));
            device.WriteByte(Address - offset + 2, (byte)(Value >> 16 & 0xff));
        }

        public int Read(int Address, int Length)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            int addr = Address - offset;
            int ret = device.ReadByte(addr);
            if (Length >= 2)
                ret += device.ReadByte(addr + 1) << 8;
            if (Length >= 3)
                ret += device.ReadByte(addr + 2) << 16;
            return ret;
        }

        internal void Write(int Address, int Value, int Length)
        {
            GetDeviceAt(Address, out Nu64.Common.IMappable device, out int offset);
            device.WriteByte(Address - offset, (byte)(Value & 0xff));
            if (Length >= 2)
                device.WriteByte(Address - offset + 1, (byte)(Value >> 8 & 0xff));
            if (Length >= 3)
                device.WriteByte(Address - offset + 2, (byte)(Value >> 16 & 0xff));
        }
    }
}
