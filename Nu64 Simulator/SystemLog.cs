﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    public static class SystemLog
    {
        public enum SeverityCodes
        {
            Fatal = 0,
            Recoverable = 1,
            Minor = 2
        }

        public static void WriteLine(SeverityCodes Severity, string Message)
        {
            System.Diagnostics.Debug.WriteLine("LOG: " + Message);
        }
    }
}
