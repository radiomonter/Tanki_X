namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public class ClassNotFoundException : Exception
    {
        public ClassNotFoundException(long id) : base("Id = " + id)
        {
        }
    }
}

