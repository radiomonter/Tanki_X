namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;

    public enum ProblemStatus
    {
        ClosedByClient,
        ClosedByServer,
        SocketMethodInvokeError,
        ReceiveError,
        DecodePacketError,
        DecodeCommandError,
        EncodeError,
        SendError,
        ExecuteCommandError
    }
}

