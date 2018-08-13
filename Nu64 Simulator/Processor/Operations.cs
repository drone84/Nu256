using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu64;

namespace Nu64.Processor
{
    /* 
     * This file contains all of the opcode routines for the Operations class. 
    */
    public class Operations
    {
        private CPU cpu;

        public Operations(CPU cPU)
        {
            this.cpu = cPU;
        }

        /// <summary>
        /// This opcode is not implemented yet. Attempting to Execute it will crash the program. 
        /// Call the kernel "abort with error message" routine. 
        /// </summary>
        public void OpNotImplemented()
        {
            cpu.Halted = true;
            cpu.OpcodeLength = 1;
            cpu.OpcodeCycles = 3;
            SystemLog.WriteLine(SystemLog.SeverityCodes.Recoverable, "Invalid Instruction (Not implemented.) CPU halted.");
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

            int address = cpu.Stack.Value;
            cpu.Memory[address] = GetByte(value, 0);
            if (bytes >= 2)
                cpu.Memory[address - 1] = GetByte(value, 1);
            if (bytes >= 3)
                cpu.Memory[address - 2] = GetByte(value, 2);
            cpu.Stack.Value -= bytes;
        }

        public void Push(Register Reg, int Offset)
        {
            Push(Reg.Value + Offset, Reg.Bytes);
        }

        public void Push(Register Reg)
        {
            Push(Reg.Value, Reg.Bytes);
        }

        public int Pull(int bytes)
        {
            if (bytes < 1 || bytes > 3)
                throw new Exception("bytes must be between 1 and 3. got " + bytes.ToString());

            cpu.Stack.Value += bytes;
            int address = cpu.Stack.Value;
            int ret = cpu.Memory[address - 1];
            if (bytes >= 2)
                ret = ret + cpu.Memory[address + 2] << 8;
            if (bytes >= 3)
                ret = ret + cpu.Memory[address + 3] << 16;

            return ret;
        }

        public void Pull(Register Register)
        {
            Register.Value = Pull(Register.Bytes);
        }

        public void OpORA(int val)
        {
            if (cpu.A.Length == Register.BitLengthEnum.Bits8)
                val = val & 0xff;

            cpu.A.Value = cpu.A.Value | val;
            cpu.Flags.SetNZ(cpu.A);
        }

        public void OpLoad(Register Dest, int value)
        {
            Dest.Value = value;
            cpu.Flags.SetNZ(Dest);
        }

        /// <summary>
        /// Branch instructions take a *signed* 8-bit value. This is added to the program counter if
        /// the test is true.
        /// </summary>
        /// <param name="b"></param>
        public void BranchNear(byte b)
        {
            int offset = MakeSignedByte(b);
            cpu.PC.Value += offset;
        }

        public sbyte MakeSignedByte(byte b)
        {
            return (sbyte)b;
        }

        public Int16 MakeSignedWord(UInt16 b)
        {
            return (Int16)b;
        }

        /// <summary>
        /// Retrieve final data from memory, based on address mode. 
        /// <para>For immediate addressing, just returns the input value</para>
        /// <para>For absolute addressing, returns data at address in signature bytes</para>
        /// <para>For indirect addressing, returns data at address pointed to by address in signature</para>
        /// <para>For indexed modes, uses appropriate index register to adjust the address</para>
        /// </summary>
        /// <param name="mode">Address mode. Direct, Absolute, Immediate, etc. Each mode determines where the data 
        /// is located and how the signature bytes are interpreted.</param>
        /// <param name="signatureBytes">byte or bytes immediately following the opcode. Varies based on the opcode.</param>
        /// <param name="isCode">Assume the address is code and uses the Program Bank Register. 
        /// Otherwise uses the Data Bank Register, if appropriate.</param>
        /// <returns></returns>
        public int GetData(AddressModes mode, int signatureBytes)
        {
            switch (mode)
            {
                case AddressModes.Accumulator:
                    return cpu.A.Value;
                case AddressModes.Absolute:
                    return GetAbsolute(signatureBytes, cpu.DataBank);
                case AddressModes.AbsoluteLong:
                    return GetAbsoluteLong(signatureBytes);
                case AddressModes.AbsoluteIndirect:
                    // JMP (addr)
                    return GetAbsoluteIndirectAddress(signatureBytes, cpu.ProgramBank);
                case AddressModes.AbsoluteIndirectLong:
                    // JMP [addr] - jumps to a 24-bit address pointed to by addr in direct page.
                    return GetAbsoluteIndirectAddressLong(signatureBytes);
                case AddressModes.AbsoluteIndexedIndirectWithX:
                    // JMP (addr,X)
                    return GetJumpAbsoluteIndexedIndirect(signatureBytes, cpu.ProgramBank, cpu.X);
                case AddressModes.AbsoluteIndexedWithX:
                    // LDA $2000,X
                    return GetIndexed(signatureBytes, cpu.DataBank, cpu.X);
                case AddressModes.AbsoluteLongIndexedWithX:
                    // LDA $12D080,X
                    return GetAbsoluteLongIndexed(signatureBytes, cpu.X);
                case AddressModes.AbsoluteIndexedWithY:
                    return GetIndexed(signatureBytes, cpu.DataBank, cpu.Y);
                case AddressModes.AbsoluteLongIndexedWithY:
                    return GetAbsoluteLongIndexed(signatureBytes, cpu.Y);
                case AddressModes.DirectPage:
                    return GetAbsolute(signatureBytes, cpu.DirectPage);
                case AddressModes.DirectPageIndexedWithX:
                    return GetIndexed(signatureBytes, cpu.DirectPage, cpu.X);
                case AddressModes.DirectPageIndexedWithY:
                    return GetIndexed(signatureBytes, cpu.DirectPage, cpu.Y);
                case AddressModes.DirectPageIndexedIndirectWithX:
                    //LDA(dp, X)
                    return GetDirectIndexedIndirect(signatureBytes, cpu.X);
                case AddressModes.DirectPageIndirect:
                    //LDA (dp)
                    return GetDirectIndirect(signatureBytes);
                case AddressModes.DirectPageIndirectIndexedWithY:
                    //LDA(dp),Y
                    return GetDirectPageIndirectIndexed(signatureBytes, cpu.Y);
                case AddressModes.DirectPageIndirectLong:
                    return GetDirectIndirectLong(signatureBytes);
                case AddressModes.DirectPageIndirectLongIndexedWithY:
                    return GetDirectPageIndirectIndexedLong(signatureBytes, cpu.Y);
                case AddressModes.ProgramCounterRelative:
                    return cpu.PC.Value + 2 + MakeSignedByte((byte)signatureBytes);
                case AddressModes.ProgramCounterRelativeLong:
                    return cpu.PC.Value + 3 + MakeSignedWord((UInt16)signatureBytes);
                case AddressModes.StackImplied:
                    return cpu.Stack.Value;
                case AddressModes.StackAbsolute:
                    return signatureBytes;
                case AddressModes.StackDirectPageIndirect:
                    throw new NotImplementedException();
                    break;
                case AddressModes.StackRelative:
                    throw new NotImplementedException();
                    break;
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    throw new NotImplementedException();
                    break;
                case AddressModes.StackProgramCounterRelativeLong:
                    throw new NotImplementedException();
                    break;
            }
            return 0;
        }

        private int GetDirectIndirect(int Address)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address);
            int ptr = cpu.Memory.ReadWord(addr);
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return cpu.Memory.ReadWord(ptr);
        }

        private int GetDirectIndirectLong(int Address)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address);
            int ptr = cpu.Memory.ReadLong(addr);
            return cpu.Memory.ReadWord(ptr);
        }

        private int GetDirectPageIndirectIndexedLong(int Address, Register Y)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address) + Y.Value;
            int ptr = cpu.Memory.ReadLong(addr);
            return cpu.Memory.ReadWord(ptr);
        }

        /// <summary>
        /// LDA (D,X) - returns value pointed to by D,X, where D is in Direct page. Final value will be in Data bank.
        /// </summary>
        /// <param name="Address">Address in direct page</param>
        /// <param name="X">Register to index</param>
        /// <returns></returns>
        private int GetDirectIndexedIndirect(int Address, Register X)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address + X.Value);
            int ptr = cpu.Memory.ReadWord(addr);
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return cpu.Memory.ReadWord(ptr);
        }

        /// <summary>
        /// LDA (D),Y - returns value pointed to by (D),Y, where D is in Direct page. Final value will be in Data bank. 
        /// </summary>
        /// <param name="Address">Address in direct page</param>
        /// <param name="X">Register to index</param>
        /// <returns></returns>
        private int GetDirectPageIndirectIndexed(int Address, Register Y)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address) + Y.Value;
            int ptr = cpu.Memory.ReadWord(addr);
            ptr = cpu.DataBank.GetLongAddress(ptr);
            return cpu.Memory.ReadWord(ptr);
        }


        private int GetAbsoluteLong(int Address)
        {
            return cpu.Memory.ReadWord(Address);
        }

        private int GetAbsoluteLongIndexed(int Address, Register Index)
        {
            return cpu.Memory.ReadWord(Address + Index.Value);
        }

        /// <summary>
        /// Read memory at specified address. Optionally use bank register 
        /// to select the relevant bank.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        private int GetAbsolute(int Address, Register bank)
        {
            return cpu.Memory.ReadWord(bank.GetLongAddress(Address));
        }

        /// <summary>
        /// LDA $2000,X
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="bank"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private int GetIndexed(int Address, Register bank, Register Index)
        {
            int addr = Address;
            addr = bank.GetLongAddress(Address);
            addr = addr + Index.Value;
            return cpu.Memory.ReadWord(addr);
        }

        /// <summary>
        /// Returns a pointer from memory. 
        /// JMP ($xxxx) reads a two-byte address from bank 0. It jumps to that address in the current
        /// program bank (meaning it adds PBR to get the final address.) 
        /// </summary>
        /// <param name="Address">Address of pointer. Final value is address pointer references.</param>
        /// <param name="block"></param>
        /// <returns></returns>
        public int GetAbsoluteIndirectAddress(int Address, Register bank)
        {
            int ptr = cpu.Memory.ReadWord(Address);
            return bank.GetLongAddress(ptr);
        }

        public int GetAbsoluteIndirectAddressLong(int Address)
        {
            int addr = cpu.DirectPage.GetLongAddress(Address);
            int ptr = cpu.Memory.ReadLong(addr);
            return cpu.Memory.ReadWord(ptr);
        }

        /// <summary>
        /// Get an indirect, indexed Jump address=: JMP ($1200,X)
        /// This looks at location $1200+X in Bank 0 to get the pointer. Then returns an address 
        /// in the current Program Bank (PBR + ($1200,X))
        /// </summary>
        /// <param name="Address">Address of pointer</param>
        /// <param name="bank">Program Bank</param>
        /// <param name="Index">Offset of address</param>
        /// <returns></returns>
        private int GetJumpAbsoluteIndexedIndirect(int Address, Register bank, Register Index)
        {
            int addr = Address + Index.Value;
            int ptr = cpu.Memory.ReadWord(addr);
            return cpu.ProgramBank.GetLongAddress(ptr);
        }

        /// <summary>
        /// BRK and COP instruction. Pushes the  Program Bank Register, the Program Counter, and the Flags onto the stack. 
        /// Then switches to the Bank 0 addresses stored in the approriate vector. 
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="addressMode"></param>
        /// <param name="signature"></param>
        public void ExecuteInterrupt(byte instruction, AddressModes addressMode, int signature)
        {
            cpu.OpcodeLength = 2;
            cpu.OpcodeCycles = 8;

            if (!cpu.Flags.Emulation)
                Push(cpu.K);
            Push(cpu.PC, 2);
            Push(cpu.P);

            int vector = MemoryMap_DirectPage.VECTOR_BRK;

            if (cpu.Flags.Emulation)
            {
                if (instruction == 0x00) // BRK
                    cpu.PC.Value = MemoryMap_DirectPage.VECTOR_EBRK;
                else if (instruction == 0x02) // COP
                    cpu.PC.Value = MemoryMap_DirectPage.VECTOR_ECOP;
            }
            else
            {
                if (instruction == 0x00) // BRK
                    cpu.PC.Value = MemoryMap_DirectPage.VECTOR_BRK;
                else if (instruction == 0x02) // COP
                    cpu.PC.Value = MemoryMap_DirectPage.VECTOR_COP;
            }
        }

        public void ExecuteORA(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteTSBTRB(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteASL(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteStack(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteBranch(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteStatusReg(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case 0x18: //CLC
                    cpu.Flags.Carry = false;
                    break;
                case 0x38: //SEC
                    cpu.Flags.Carry = true;
                    break;
                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteStatusReg: " + instruction.ToString());
            }

            cpu.SyncFlags();
        }

        public void ExecuteINCDEC(byte instruction, AddressModes addressMode, int signature)
        {
            byte bval = (byte)GetData(addressMode, signature);
            int addr = GetAddress(addressMode, signature);

            switch (instruction)
            {
                case OpcodeList.DEC_Accumulator:
                    cpu.A.Value -= 1;
                    cpu.Flags.SetNZ(cpu.A);
                    break;
                case OpcodeList.DEC_DirectPage:
                case OpcodeList.DEC_Absolute:
                case OpcodeList.DEC_DirectPageIndexedWithX:
                case OpcodeList.DEC_AbsoluteIndexedWithX:
                    bval--;
                    cpu.Memory.WriteByte(addr, bval);
                    cpu.Flags.SetNZ(bval, Register.BitLengthEnum.Bits8);
                    break;

                case OpcodeList.INC_Accumulator:
                    cpu.A.Value += 1;
                    cpu.Flags.SetNZ(cpu.A);
                    break;
                case OpcodeList.INC_DirectPage:
                case OpcodeList.INC_Absolute:
                case OpcodeList.INC_DirectPageIndexedWithX:
                case OpcodeList.INC_AbsoluteIndexedWithX:
                    addr = cpu.DirectPage.GetLongAddress(addr);
                    bval++;
                    cpu.Memory.WriteByte(addr, bval);
                    cpu.Flags.SetNZ(bval, Register.BitLengthEnum.Bits8);
                    break;

                case OpcodeList.DEX_Implied:
                    cpu.X.Value -= 1;
                    cpu.Flags.SetNZ(cpu.X);
                    break;
                case OpcodeList.DEY_Implied:
                    cpu.X.Value -= 1;
                    cpu.Flags.SetNZ(cpu.X);
                    break;
                case OpcodeList.INX_Implied:
                    cpu.X.Value += 1;
                    cpu.Flags.SetNZ(cpu.X);
                    break;
                case OpcodeList.INY_Implied:
                    cpu.Y.Value += 1;
                    cpu.Flags.SetNZ(cpu.Y);
                    break;
                default:
                    break;
            }


        }

        private int GetAddress(AddressModes addressMode, int SignatureBytes)
        {
            int addr = 0;
            int ptr = 0;
            switch (addressMode)
            {
                case AddressModes.Absolute:
                    return cpu.DataBank.GetLongAddress(SignatureBytes);
                case AddressModes.AbsoluteLong:
                    return SignatureBytes;
                case AddressModes.AbsoluteIndirect:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.AbsoluteIndirectLong:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadLong(addr);
                    return ptr;
                case AddressModes.AbsoluteIndexedIndirectWithX:
                    break;
                case AddressModes.AbsoluteIndexedWithX:
                    return cpu.DataBank.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.AbsoluteLongIndexedWithX:
                    break;
                case AddressModes.AbsoluteIndexedWithY:
                    break;
                case AddressModes.AbsoluteLongIndexedWithY:
                    break;
                case AddressModes.DirectPage:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes);
                case AddressModes.DirectPageIndexedWithX:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.DirectPageIndexedWithY:
                    break;
                case AddressModes.DirectPageIndexedIndirectWithX:
                    break;
                case AddressModes.DirectPageIndirect:
                    break;
                case AddressModes.DirectPageIndirectIndexedWithY:
                    break;
                case AddressModes.DirectPageIndirectLong:
                    break;
                case AddressModes.DirectPageIndirectLongIndexedWithY:
                    break;
                case AddressModes.ProgramCounterRelative:
                    break;
                case AddressModes.ProgramCounterRelativeLong:
                    break;
                case AddressModes.StackImplied:
                    break;
                case AddressModes.StackAbsolute:
                    break;
                case AddressModes.StackDirectPageIndirect:
                    break;
                case AddressModes.StackRelative:
                    break;
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    break;
                case AddressModes.StackProgramCounterRelativeLong:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException("GetAddress() Address mode not implemented: " + addressMode.ToString());
        }

        public void ExecuteTransfer(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteJumpReturn(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteAND(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteBIT(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteROL(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteEOR(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteMisc(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteLSR(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteADC(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteSTZ(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteROR(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteSTA(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteSTY(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteSTX(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteLDY(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteLDA(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteLDX(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteCPXCPY(byte instruction, AddressModes addressMode, int signature)
        {
        }


        public void ExecuteCMP(byte instruction, AddressModes addressMode, int signature)
        {
        }


        public void ExecuteSBC(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteWAI(byte Instruction, AddressModes AddressMode, int Signature)
        {
        }
    }
}
