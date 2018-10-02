﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Processor
{
    public enum AddressModes
    {
        Accumulator,
        BlockMove,
        Immediate,
        Implied,
        Interrupt,

        Absolute,
        AbsoluteLong,
        JmpAbsoluteIndirect,
        JmpAbsoluteIndirectLong,
        JmpAbsoluteIndexedIndirectWithX,
        AbsoluteIndexedWithX,
        AbsoluteLongIndexedWithX,
        AbsoluteIndexedWithY,
        AbsoluteLongIndexedWithY,

        DirectPage,
        DirectPageIndexedWithX,
        DirectPageIndexedWithY,
        DirectPageIndexedIndirectWithX,
        DirectPageIndirect,
        DirectPageIndirectIndexedWithY,
        DirectPageIndirectLong,
        DirectPageIndirectLongIndexedWithY,

        ProgramCounterRelative,
        ProgramCounterRelativeLong,

        StackImplied,
        //StackAbsolute,
        StackDirectPageIndirect,
        StackRelative,
        StackRelativeIndirectIndexedWithY,
        StackProgramCounterRelativeLong,
    }

}
