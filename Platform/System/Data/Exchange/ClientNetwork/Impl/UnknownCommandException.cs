namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;

    public class UnknownCommandException : Exception
    {
        public UnknownCommandException(CommandCode commandCode) : base($"Unknown command code {commandCode}.")
        {
        }
    }
}

