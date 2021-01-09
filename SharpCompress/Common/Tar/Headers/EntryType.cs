namespace SharpCompress.Common.Tar.Headers
{
    using System;

    internal enum EntryType : byte
    {
        File = 0,
        OldFile = 0x30,
        HardLink = 0x31,
        SymLink = 50,
        CharDevice = 0x33,
        BlockDevice = 0x34,
        Directory = 0x35,
        Fifo = 0x36,
        LongLink = 0x4b,
        LongName = 0x4c,
        SparseFile = 0x53,
        VolumeHeader = 0x56
    }
}

