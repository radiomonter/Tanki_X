namespace log4net.Core
{
    using System;

    public enum ErrorCode
    {
        GenericFailure,
        WriteFailure,
        FlushFailure,
        CloseFailure,
        FileOpenFailure,
        MissingLayout,
        AddressParseFailure
    }
}

