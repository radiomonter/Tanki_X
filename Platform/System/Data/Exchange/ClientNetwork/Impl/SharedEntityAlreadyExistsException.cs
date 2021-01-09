namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;

    public class SharedEntityAlreadyExistsException : Exception
    {
        public SharedEntityAlreadyExistsException(long id) : base("Entity with ID " + id + " already exists in engine.")
        {
        }
    }
}

