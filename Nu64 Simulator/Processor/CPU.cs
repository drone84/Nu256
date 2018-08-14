﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu64;

namespace Nu64.Processor
{
    /// <summary>
    /// Operations. This class encompasses the CPU operations and the support routines needed to execute
    /// the operaitons. Execute reads a single opcode from memory, along with its data bytes, then 
    /// executes that single step. The virtual CPU state is retained until Execute is called again. 
    /// </summary>
    public partial class CPU
    {
        const int BANKSIZE = 0x10000;
        const int PAGESIZE = 0x100;
        private OpcodeList opcodes = null;
        private Operations operations = null;

        /// <summary>
        /// Currently executing opcode 
        /// </summary>
        public byte Instruction = 0;
        public OpCode OC = null;
        public int SignatureBytes = 0;

        /// <summary>
        /// Length of the currently executing opcode
        /// </summary>
        public int OpcodeLength;
        /// <summary>
        /// Number of clock cycles used by the currently exeucting instruction
        /// </summary>
        public int OpcodeCycles;

        /// <summary>
        ///  The virtual CPU speed
        /// </summary>
        private int clockSpeed = 14000000;
        /// <summary>
        /// number of cycles since the last performance checkpopint
        /// </summary>
        private int clockCyles = 0;
        /// <summary>
        /// the number of cycles to pause at until the next performance checkpoint
        /// </summary>
        private long nextCycleCheck = long.MaxValue;

        public SystemBus Memory = null;

        public bool Halted = false;

        public int ClockSpeed
        {
            get
            {
                return this.clockSpeed;
            }

            set
            {
                this.clockSpeed = value;
            }
        }

        public CPU(SystemBus newMemory)
        {
            this.Memory = newMemory;
            this.clockSpeed = 14000000;
            this.clockCyles = 0;
            this.operations = new Operations(this);
            this.opcodes = new OpcodeList(this.operations, this);
        }

        public void Start(int Address)
        {
            Halted = false;
            SetPC(Address);
            ExecuteNext();
        }

        public void ExecuteNext()
        {
            Instruction = GetNextInstruction();
            Decode(Instruction);
            OC.Execute(SignatureBytes);
            PC.Value += OpcodeLength;
            clockCyles += OpcodeCycles;
        }

        public void Decode(byte instruction)
        {
            OC = opcodes[Instruction];
            OpcodeLength = OC.Length;
            OpcodeCycles = 3;

            int s = 0;
            if (OC.Length == 2)
                s = GetNextByte(0);
            else if (OC.Length == 3)
                s = GetNextWord(0);
            else if (OC.Length == 4)
                s = GetNextLong(0);
            SignatureBytes = s;
        }

        private byte GetNextInstruction()
        {
            int address = GetLongPC();
            byte ret = Memory[address];
            return ret;
        }

        /// <summary>
        /// Retrieves the next byte from the instruction stream. 
        /// Set offset=0 for the first byte after the executing opcode.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private byte GetNextByte(int offset = 0)
        {
            int address = GetLongPC();
            return Memory.ReadByte(address + offset + 1);
        }

        /// <summary>
        /// Retrieves the next byte from the instruction stream. 
        /// Set offset=0 for the first byte after the executing opcode.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private int GetNextWord(int offset)
        {
            int address = GetLongPC();
            return Memory.ReadWord(address + offset + 1);
        }

        private int GetNextLong(int offset)
        {
            int address = GetLongPC();
            return Memory.ReadLong(address + offset + 1);
        }

        /// <summary>
        /// Clock cycles used for performance counte This will be periodically reset to zero
        /// as the throttling routine adjusts the system performance. 
        /// </summary>
        public int CycleCounter
        {
            get { return this.clockCyles; }
        }

        /// <summary>
        /// Execute the instruction specified by Opcode, retrieving additional bytes from the instruction
        /// stream as necessary. opLength will be set to the number of bytes read. OpcodeCycles will be set
        /// to the number of CPU cycles used. 
        /// </summary>
        /// <param name="Opcode">Opcode to execute</param>
        private void Execute(byte opcodeByte)
        {

            #region old Opcode Implementation
            //switch (Opcode)
            //{
            //    case 0x00: //BRK
            //        operations.OpBRK();
            //        break;
            //    case 0x01: // ORA (d,x)
            //        OpcodeLength = 2;
            //        addr = GetPointerDirect(GetNextByte(0), X);
            //        val = Memory[addr];
            //        operations.OpORA(val);
            //        break;
            //    case 0x02: //COP d
            //        OpcodeLength = 2;
            //        OpcodeCycles = 7;
            //        operations.OpBRK(true);
            //        break;
            //    case 0x03: // ORA d,s
            //        OpcodeLength = 2;
            //        addr = GetStackValue(GetNextByte(0));
            //        val = Memory.ReadWord(addr);
            //        operations.OpORA(val);
            //        break;
            //    case 0x04: // TSB,d
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x05: // ORA d
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x06: // ASL d
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x07: // ORA [d]
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x08: // PHP s
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x09: // ORA #
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x0a: // ASL A
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x0b: // PHD s
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x0c: // TSB a 
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x0d: // ORA a
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x0e: // ASL a
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x0f: // ORA al
            //        operations.OpNotImplemented();
            //        break;

            //    case 0x10: // BPL r Branch Plus (Branch if Negative flag not set)
            //        OpcodeLength = 2;
            //        OpcodeCycles = 2;
            //        if (!Flags.Negative)
            //            operations.BranchNear(GetNextByte());
            //        break;
            //    case 0x11: // ORA (d,x)
            //        OpcodeLength = 2;
            //        addr = GetPointerDirect(GetNextByte(0), X);
            //        val = Memory[addr];
            //        operations.OpORA(val);
            //        break;
            //    case 0x12: //COP d
            //        OpcodeLength = 2;
            //        OpcodeCycles = 7;
            //        operations.OpBRK(true);
            //        break;
            //    case 0x13: // ORA d,s
            //        OpcodeLength = 2;
            //        addr = GetStackValue(GetNextByte(0));
            //        val = Memory.ReadWord(addr);
            //        operations.OpORA(val);
            //        break;
            //    case 0x14: // TSB,d
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x15: // ORA d
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x16: // ASL d
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x17: // ORA [d]
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x18: //CLC clear carry
            //        OpcodeLength = 1;
            //        OpcodeCycles = 2;
            //        Flags.Carry = false;
            //        break;
            //    case 0x19: // ORA #
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x1a: // ASL A
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x1b: // PHD s
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x1c: // TSB a 
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x1d: // ORA a
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x1e: // ASL a
            //        operations.OpNotImplemented();
            //        break;
            //    case 0x1f: // ORA al
            //        operations.OpNotImplemented();
            //        break;

            //    case 0x38: //SEC set carry
            //        OpcodeLength = 1;
            //        OpcodeCycles = 2;
            //        Flags.Carry = true;
            //        break;

            //    case 0xa0: // LDY d
            //        OpcodeLength = Y.Length == Register.BitLengthEnum.Bits8 ? 2 : 3;
            //        OpcodeCycles = Y.Length == Register.BitLengthEnum.Bits8 ? 2 : 3;
            //        val = GetNextWord(0);
            //        operations.OpLoad(Y, val);
            //        break;

            //    case 0xa2: // LDX d
            //        OpcodeLength = X.Length == Register.BitLengthEnum.Bits8 ? 2 : 3;
            //        OpcodeCycles = X.Length == Register.BitLengthEnum.Bits8 ? 2 : 3;
            //        val = GetNextWord(0);
            //        operations.OpLoad(X, val);
            //        break;

            //    case 0xa9: // LDA d
            //        OpcodeLength = A.Length == Register.BitLengthEnum.Bits8 ? 2 : 3;
            //        OpcodeCycles = A.Length == Register.BitLengthEnum.Bits8 ? 2 : 3;
            //        val = GetNextWord(0);
            //        operations.OpLoad(A, val);
            //        break;

            //    case 0xc2: // REP d 
            //        // reset the flags using the bit pattern in the operator
            //        // ex: REP $01 turns OFF the carry flag (bit 0)
            //        OpcodeLength = 2;
            //        OpcodeCycles = 3;
            //        val = GetNextByte(0);
            //        Flags.Value = Flags.Value | val;
            //        Flags.Value = Flags.Value ^ val;
            //        SyncRegisterWidth();
            //        break;

            //    case 0xe2: // SEP d 
            //        // set the flags using the bit pattern in the operator
            //        // ex: SEP $01 turns ON the carry flag (bit 0)
            //        OpcodeLength = 2;
            //        OpcodeCycles = 3;
            //        val = GetNextByte(0);
            //        Flags.Value = Flags.Value | val;
            //        SyncRegisterWidth();
            //        break;

            //    case 0xdb: // STP 
            //        OpcodeLength = 1;
            //        OpcodeCycles = 3;
            //        Halted = true;
            //        break;

            //    case 0xfb: // XCE eXchange Carry and Emulation bits
            //        OpcodeLength = 1;
            //        OpcodeCycles = 2;
            //        Flags.SwapCE();
            //        SyncRegisterWidth();
            //        break;

            //    default:
            //        operations.OpNotImplemented();
            //        break;
            //}
            #endregion

            if (OpcodeLength == 0)
                throw new Exception("OpcodeLength must be >0, got " + OpcodeLength.ToString());
        }

        #region support routines
        /// <summary>
        /// Gets the address pointed to by a pointer in the data bank.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerLocal(int baseAddress, Register Index = null)
        {
            int addr = DataBank.GetLongAddress(baseAddress);
            if (Index != null)
                addr += Index.Value;
            return addr;
        }

        /// <summary>
        /// Gets the address pointed to by a pointer in Direct page.
        /// be in the Direct Page. The address returned will be DBR+Pointer.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerDirect(int baseAddress, Register Index = null)
        {
            int addr = DirectPage.Value + baseAddress;
            if (Index != null)
                addr += Index.Value;
            int pointer = Memory.ReadWord(addr);
            return DataBank.GetLongAddress(pointer);
        }

        /// <summary>
        /// Gets the address pointed to by a pointer referenced by a long address.
        /// </summary>
        /// <param name="baseAddress">24-bit address</param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetPointerLong(int baseAddress, Register Index = null)
        {
            int addr = baseAddress;
            if (Index != null)
                addr += Index.Value;
            return DataBank.GetLongAddress(Memory.ReadWord(addr));
        }

        /// <summary>
        /// Returns a value from the stack. 
        /// </summary>
        /// <param name="Offset">Number of bytes below stack pointer to read.</param>
        /// <returns></returns>
        private int GetStackValue(int Offset = 0)
        {
            int addr = Stack.Value - Offset;
            return Memory.ReadWord(addr);
        }

        #endregion

        /// <summary>
        /// Change execution to anohter address in the same bank
        /// </summary>
        /// <param name="addr"></param>
        public void JumpShort(int addr)
        {
            PC.Value = addr;
        }

        /// <summary>
        /// Change execution to a 24-bit address
        /// </summary>
        /// <param name="addr"></param>
        public void JumpLong(int addr)
        {
            ProgramBank.Value = addr >> 16;
            PC.Value = addr;
        }

        public void JumpVector(int VectorAddress)
        {
            int addr = Memory.ReadWord(VectorAddress);
            ProgramBank.Value = 0;
            PC.Value = addr;
        }

        public byte GetByte(int Value, int Offset)
        {
            if (Offset == 0)
                return (byte)(Value & 0xff);
            if (Offset == 1)
                return (byte)(Value >> 8 & 0xff);
            if (Offset == 2)
                return (byte)(Value >> 16 & 0xff);

            throw new Exception("Offset must be 0-2. Got " + Offset.ToString());
        }

        public void Push(int value, int bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. got " + bytes.ToString());

            int address = Stack.Value;
            Memory[address] = GetByte(value, 0);
            if (bytes >= 2)
                Memory[address - 1] = GetByte(value, 1);
            if (bytes >= 3)
                Memory[address - 2] = GetByte(value, 2);
            Stack.Value -= bytes;
        }

        public void Push(Register Reg, int Offset)
        {
            Push(Reg.Value + Offset, Reg.Bytes);
        }

        public void Push(Register Reg)
        {
            Push(Reg.Value, Reg.Bytes);
        }

        public int Pop(int bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. got " + bytes.ToString());

            Stack.Value += bytes;
            int address = Stack.Value;
            int ret = Memory[address - 1];
            if (bytes >= 2)
                ret = ret + Memory[address + 2] << 8;
            if (bytes >= 3)
                ret = ret + Memory[address + 3] << 16;

            return ret;
        }

        public void PopInto(Register Register)
        {
            Register.Value = Pop(Register.Bytes);
        }

        public void Interrupt(InteruptTypes T)
        {
            if (!Flags.Emulation)
                Push(ProgramBank);
            Push(PC, 2);
            Push(Flags);

            int addr = MemoryMap_DirectPage.VECTOR_BRK;
            int eaddr = MemoryMap_DirectPage.VECTOR_EBRK;
            switch (T)
            {
                case InteruptTypes.ABORT:
                    eaddr = MemoryMap_DirectPage.VECTOR_EABORT;
                    addr = MemoryMap_DirectPage.VECTOR_ABORT;
                    break;
                case InteruptTypes.IRQ:
                    eaddr = MemoryMap_DirectPage.VECTOR_EIRQ;
                    addr = MemoryMap_DirectPage.VECTOR_IRQ;
                    break;
                case InteruptTypes.NMI:
                    eaddr = MemoryMap_DirectPage.VECTOR_ENMI;
                    addr = MemoryMap_DirectPage.VECTOR_NMI;
                    break;
                case InteruptTypes.RESET:
                    eaddr = MemoryMap_DirectPage.VECTOR_ERESET;
                    addr = MemoryMap_DirectPage.VECTOR_RESET;
                    break;
                case InteruptTypes.COP:
                    eaddr = MemoryMap_DirectPage.VECTOR_ECOP;
                    addr = MemoryMap_DirectPage.VECTOR_COP;
                    break;
            }

            if (Flags.Emulation)
                JumpVector(eaddr);
            else
                JumpVector(addr);
        }
    }
}
