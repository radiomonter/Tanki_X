namespace WebSocketSharp
{
    using System;

    public enum CloseStatusCode : ushort
    {
        Normal = 0x3e8,
        Away = 0x3e9,
        ProtocolError = 0x3ea,
        UnsupportedData = 0x3eb,
        Undefined = 0x3ec,
        NoStatus = 0x3ed,
        Abnormal = 0x3ee,
        InvalidData = 0x3ef,
        PolicyViolation = 0x3f0,
        TooBig = 0x3f1,
        MandatoryExtension = 0x3f2,
        ServerError = 0x3f3,
        TlsHandshakeFailure = 0x3f7
    }
}

