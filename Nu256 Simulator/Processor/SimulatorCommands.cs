﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Processor
{
    public static class SimulatorCommands
    {
        /// <summary>
        /// Tell the CPU thread to pause until an interrupt is received
        /// </summary>
        public const int WaitForInterrupt = 0;
        /// <summary>
        /// Tell the display to refresh the screen. The screen will refresh at the next 60Hz interval.
        /// </summary>
        public const int RefreshDisplay = 1;
    }
}
