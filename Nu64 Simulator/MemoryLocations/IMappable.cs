﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    public interface IMappable
    {
        /// <summary>
        /// Read a byte from memory or device. Address is LOCAL to this device. Note that I/O devices have separate input and output registers,
        /// so the byte being read may not be the same data as the byte being written.
        /// </summary>
        /// <param name="Address">Address in this device's memory or I/O array. 0 is always the first location on this device.</param>
        /// <returns>byte of data at this location. </returns>
        byte ReadByte(int Address);
        /// <summary>
        /// Write a byte of data to memory or to a memory mapped device.  Note that I/O devices have separate input and output registers,
        /// so the byte being read may not be the same data as the byte being written.
        /// </summary>
        /// <param name="Address">Address in this device's memory or I/O array. 0 is always the first location on this device.</param>
        /// <param name="Data">Data to write to memory or I/O</param>
        /// <returns></returns>
        void WriteByte(int Address, byte Data);
    }
}
