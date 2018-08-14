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

        public void Reset()
        {
            cpu.A.Reset();
            cpu.X.Reset();
            cpu.Y.Reset();
            cpu.Flags.Reset();
            cpu.DataBank.Reset();
            cpu.DirectPage.Reset();
            cpu.ProgramBank.Reset();
            cpu.PC.Reset();

            cpu.PC.Value = cpu.Memory.ReadWord(MemoryMap_DirectPage.VECTOR_RESET);
        }

        /// <summary>
        /// This opcode is not implemented yet. 
        /// </summary>
        public void ExecuteAbort()
        {
            cpu.OpcodeLength = 1;
            cpu.OpcodeCycles = 1;
            SystemLog.WriteLine(SystemLog.SeverityCodes.Recoverable, "Invalid Instruction (Not implemented.) Abort processed."
                + "\r\nPC: " + cpu.ProgramBank.GetLongAddress(cpu.PC)
                + "\r\ninstruction: " + cpu.OC.ToString());


            cpu.Halted = true;
        }

        public void OpORA(int val)
        {
            if (cpu.A.Width == 1)
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

        public Int16 MakeSignedInt(UInt16 b)
        {
            return (Int16)b;
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
        public int GetData(AddressModes mode, int signatureBytes, int bytes = 2)
        {
            switch (mode)
            {
                case AddressModes.Accumulator:
                    return cpu.A.Value;
                case AddressModes.Absolute:
                    return GetAbsolute(signatureBytes, cpu.DataBank);
                case AddressModes.AbsoluteLong:
                    return GetAbsoluteLong(signatureBytes);
                case AddressModes.JmpAbsoluteIndirect:
                    // JMP (addr)
                    return GetAbsoluteIndirectAddress(signatureBytes, cpu.ProgramBank);
                case AddressModes.JmpAbsoluteIndirectLong:
                    // JMP [addr] - jumps to a 24-bit address pointed to by addr in direct page.
                    return GetAbsoluteIndirectAddressLong(signatureBytes);
                case AddressModes.JmpAbsoluteIndexedIndirectWithX:
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
                case AddressModes.StackRelative:
                    throw new NotImplementedException();
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    throw new NotImplementedException();
                case AddressModes.StackProgramCounterRelativeLong:
                    throw new NotImplementedException();
            }
            return signatureBytes;
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
        private int GetAbsolute(int Address, Register bank, int Bytes = 2)
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

            switch (instruction)
            {
                case OpcodeList.BRK_Interrupt:
                    cpu.Interrupt(InteruptTypes.BRK);
                    break;
                case OpcodeList.COP_Interrupt:
                    cpu.Interrupt(InteruptTypes.COP);
                    break;
                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteInterrupt: " + instruction.ToString("X2"));
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
                case OpcodeList.CLC_Implied:
                    cpu.Flags.Carry = false;
                    break;
                case OpcodeList.SEC_Implied:
                    cpu.Flags.Carry = true;
                    break;
                case OpcodeList.CLD_Implied:
                    cpu.Flags.Decimal = false;
                    break;
                case OpcodeList.SED_Implied:
                    cpu.Flags.Decimal = true;
                    break;
                case OpcodeList.CLI_Implied:
                    cpu.Flags.Irqdisable = false;
                    break;
                case OpcodeList.SEI_Implied:
                    cpu.Flags.Irqdisable = true;
                    break;
                case OpcodeList.CLV_Implied:
                    cpu.Flags.oVerflow = false;
                    break;
                case OpcodeList.REP_Immediate:
                    // reset (clear) flag bits that are 1 in the argument
                    // do this by flipping the argument bits, then ANDing 
                    // them to the flag bits 
                    int flip = signature ^ 0xff;
                    cpu.Flags.Value = cpu.Flags.Value & flip;
                    break;
                case OpcodeList.SEP_Immediate:
                    // set flag bits that are 1 in the argument. 
                    cpu.Flags.Value = cpu.Flags.Value | signature;
                    break;
                case OpcodeList.XCE_Implied:
                    cpu.Flags.SwapCE();
                    break;
                default:
                    throw new NotImplementedException("Unknown opcode for ExecuteStatusReg: " + instruction.ToString("X2"));
            }

            cpu.SyncFlags();
        }

        public void ExecuteINCDEC(byte instruction, AddressModes addressMode, int signature)
        {
            byte bval = (byte)GetData(addressMode, signature);
            int addr = GetAddress(addressMode, signature, cpu.DataBank);

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
                    cpu.Flags.SetNZ(bval, 1);
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
                    cpu.Flags.SetNZ(bval, 1);
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

        private int GetAddress(AddressModes addressMode, int SignatureBytes, RegisterBankNumber Bank)
        {
            int addr = 0;
            int ptr = 0;
            switch (addressMode)
            {
                case AddressModes.Absolute:
                    return Bank.GetLongAddress(SignatureBytes);
                case AddressModes.AbsoluteLong:
                    return SignatureBytes;
                case AddressModes.AbsoluteIndexedWithX:
                    return Bank.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.AbsoluteLongIndexedWithX:
                    return SignatureBytes + cpu.X.Value;
                case AddressModes.AbsoluteIndexedWithY:
                    return SignatureBytes + cpu.Y.Value;
                case AddressModes.AbsoluteLongIndexedWithY:
                    return Bank.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.DirectPage:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes);
                case AddressModes.DirectPageIndexedWithX:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes + cpu.X.Value);
                case AddressModes.DirectPageIndexedWithY:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes + cpu.Y.Value);
                case AddressModes.DirectPageIndexedIndirectWithX:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes) + cpu.X.Value;
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.DirectPageIndirect:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.DirectPageIndirectIndexedWithY:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadWord(addr) + cpu.Y.Value;
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.DirectPageIndirectLong:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadLong(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.DirectPageIndirectLongIndexedWithY:
                    addr = cpu.DirectPage.GetLongAddress(SignatureBytes);
                    ptr = cpu.Memory.ReadLong(addr) + cpu.Y.Value;
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.ProgramCounterRelative:
                    ptr = MakeSignedByte((byte)SignatureBytes);
                    addr = cpu.PC.Value + ptr;
                    return addr;
                case AddressModes.ProgramCounterRelativeLong:
                    ptr = MakeSignedInt((UInt16)SignatureBytes);
                    addr = cpu.PC.Value + ptr;
                    return addr;
                case AddressModes.StackImplied:
                case AddressModes.StackAbsolute:
                    return 0;
                case AddressModes.StackDirectPageIndirect:
                    return cpu.DirectPage.GetLongAddress(SignatureBytes);
                case AddressModes.StackRelative:
                    return cpu.Stack.Value + SignatureBytes;
                case AddressModes.StackRelativeIndirectIndexedWithY:
                    return cpu.Stack.Value + SignatureBytes + cpu.Y.Value;
                case AddressModes.StackProgramCounterRelativeLong:
                    return SignatureBytes;

                // Jump and JSR indirect references vectors located in Bank 0
                case AddressModes.JmpAbsoluteIndirect:
                    addr = SignatureBytes;
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);
                case AddressModes.JmpAbsoluteIndirectLong:
                    addr = SignatureBytes;
                    ptr = cpu.Memory.ReadLong(addr);
                    return ptr;
                case AddressModes.JmpAbsoluteIndexedIndirectWithX:
                    addr = SignatureBytes + cpu.X.Value;
                    ptr = cpu.Memory.ReadWord(addr);
                    return cpu.ProgramBank.GetLongAddress(ptr);

                default:
                    throw new NotImplementedException("GetAddress() Address mode not implemented: " + addressMode.ToString());
            }
        }

        public void ExecuteTransfer(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case OpcodeList.TCD_Implied:
                    cpu.DirectPage.Value = cpu.A.Value16;
                    break;
                case OpcodeList.TDC_Implied:
                    cpu.A.Value16 = cpu.DirectPage.Value;
                    break;
                case OpcodeList.TCS_Implied:
                    cpu.Stack.Value = cpu.A.Value16;
                    break;
                case OpcodeList.TSC_Implied:
                    cpu.A.Value16 = cpu.Stack.Value;
                    break;
                case OpcodeList.TAX_Implied:
                    cpu.X.Value = cpu.A.Value16;
                    break;
                case OpcodeList.TAY_Implied:
                    cpu.Y.Value = cpu.A.Value16;
                    break;
                case OpcodeList.TSX_Implied:
                    cpu.X.Value = cpu.Stack.Value;
                    break;
                case OpcodeList.TXA_Implied:
                    cpu.A.Value = cpu.X.Value;
                    break;
                case OpcodeList.TXS_Implied:
                    cpu.Stack.Value = cpu.X.Value;
                    break;
                case OpcodeList.TXY_Implied:
                    cpu.Y.Value = cpu.X.Value;
                    break;
                case OpcodeList.TYA_Implied:
                    cpu.A.Value = cpu.Y.Value;
                    break;
                case OpcodeList.TYX_Implied:
                    cpu.X.Value = cpu.Y.Value;
                    break;
                default:
                    throw new NotImplementedException("ExecuteTransfer() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteJumpReturn(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.ProgramBank);
            switch (instruction)
            {
                case OpcodeList.JSR_Absolute:
                case OpcodeList.JSR_AbsoluteIndexedIndirectWithX:
                    cpu.Push(cpu.PC);
                    cpu.JumpShort(addr);
                    return;
                case OpcodeList.JSR_AbsoluteLong:
                    cpu.Push(cpu.ProgramBank);
                    cpu.Push(cpu.PC);
                    cpu.JumpLong(addr);
                    return;
                case OpcodeList.JMP_Absolute:
                case OpcodeList.JMP_AbsoluteLong:
                case OpcodeList.JMP_AbsoluteIndirect:
                case OpcodeList.JMP_AbsoluteIndexedIndirectWithX:
                case OpcodeList.JMP_AbsoluteIndirectLong:
                    cpu.JumpLong(addr);
                    return;
                case OpcodeList.RTS_StackImplied:
                    cpu.JumpShort(cpu.Pop(2));
                    return;
                case OpcodeList.RTL_StackImplied:
                    cpu.JumpLong(cpu.Pop(3));
                    return;
                default:
                    throw new NotImplementedException("ExecuteJumpReturn() opcode not implemented: " + instruction.ToString("X2"));
            }
        }

        public void ExecuteAND(byte instruction, AddressModes addressMode, int signature)
        {
            int data = GetData(addressMode, signature);
            cpu.A.Value = cpu.A.Value & data;
            cpu.Flags.SetNZ(cpu.A);
        }

        public void ExecuteBIT(byte instruction, AddressModes addressMode, int signature)
        {
            int data = GetData(addressMode, signature);
            int result = cpu.A.Value & data;
            if (addressMode != AddressModes.Immediate)
            {
                cpu.Flags.SetNZ(result, cpu.A.Width);
                if (cpu.A.Width == 2)
                    cpu.Flags.oVerflow = (result & 0x4000) == 0x4000;
                else
                    cpu.Flags.oVerflow = (result & 0x400) == 0x40;
            }
            else
                cpu.Flags.SetZ(result);
        }

        public void ExecuteROL(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteEOR(byte instruction, AddressModes addressMode, int signature)
        {
        }

        public void ExecuteMisc(byte instruction, AddressModes addressMode, int signature)
        {
            switch (instruction)
            {
                case OpcodeList.STP_Implied: //stop
                    cpu.Halted = true;
                    break;
                default:
                    throw new NotImplementedException("ExecuteJumpReturn() opcode not implemented: " + instruction.ToString("X2"));
                    break;
            }
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
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            cpu.Memory.Write(addr, cpu.A.Value, cpu.A.Width);
        }

        public void ExecuteSTY(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            cpu.Memory.Write(addr, cpu.Y.Value, cpu.Y.Width);
        }

        public void ExecuteSTX(byte instruction, AddressModes addressMode, int signature)
        {
            int addr = GetAddress(addressMode, signature, cpu.DataBank);
            cpu.Memory.Write(addr, cpu.X.Value, cpu.X.Width);
        }

        public void ExecuteLDY(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetData(addressMode, signature);
            cpu.Y.Value = val;
        }

        public void ExecuteLDA(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetData(addressMode, signature);
            cpu.A.Value = val;
        }

        public void ExecuteLDX(byte instruction, AddressModes addressMode, int signature)
        {
            int val = GetData(addressMode, signature);
            cpu.X.Value = val;
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
            cpu.Halted = true;
        }
    }
}
