namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public interface CommandsCodec : Codec
    {
        void RegisterCommand<T>(CommandCode code) where T: Command;
    }
}

