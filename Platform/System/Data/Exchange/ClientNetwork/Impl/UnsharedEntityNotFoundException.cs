namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;

    public class UnsharedEntityNotFoundException : Exception
    {
        public UnsharedEntityNotFoundException(long id) : base("id=" + id)
        {
        }
    }
}

