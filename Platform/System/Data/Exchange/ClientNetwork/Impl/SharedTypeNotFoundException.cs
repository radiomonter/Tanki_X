namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;

    public class SharedTypeNotFoundException : Exception
    {
        public SharedTypeNotFoundException(long id, Type type) : base($"Shared type with UID {id} was not found in registry {type}.")
        {
        }
    }
}

