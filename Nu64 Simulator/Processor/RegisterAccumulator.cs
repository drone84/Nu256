﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public class RegisterAccumulator : Register
    {
        public int Value16
        {
            get { return this._value; }
            set { this._value = value; }
        }
    }
}
