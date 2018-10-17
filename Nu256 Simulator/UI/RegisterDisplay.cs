﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nu256.Simulator.Processor;

namespace Nu256.Simulator
{
    public partial class RegisterDisplay : UserControl
    {
        public RegisterDisplay()
        {
            InitializeComponent();
        }

        private CPU _cpu;
        public CPU CPU
        {
            get { return this._cpu; }
            set
            {
                this._cpu = value;
                SetRegisters();
            }

        }

        private void SetRegisters()
        {
            if (_cpu != null)
            {
                this.PC.Register = _cpu.PC;
                this.PC.Bank = _cpu.ProgramBank;
                this.A.Register = _cpu.A;
                this.X.Register = _cpu.X;
                this.Y.Register = _cpu.Y;
                this.Stack.Register = _cpu.Stack;
                this.DBR.Register = _cpu.DataBank;
                this.D.Register = _cpu.DirectPage;
                this.Flags.Register = _cpu.Flags;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (object c in this.groupBox1.Controls)
            {
                if (c is UI.RegisterControl rc)
                    rc.UpdateValue();
            }
        }
    }
}
