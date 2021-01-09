namespace Platform.Library.ClientProtocol.Impl
{
    using System;

    public class TypeByUidNotFoundException : Exception
    {
        public TypeByUidNotFoundException(long uid) : base("uid = " + uid)
        {
        }
    }
}

