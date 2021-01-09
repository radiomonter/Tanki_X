namespace Platform.Library.ClientProtocol.Impl
{
    using System;

    public class TypeWithSameUidAlreadyRegisteredException : Exception
    {
        public TypeWithSameUidAlreadyRegisteredException(long uid, Type existsType, Type type) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "uid = ", uid, ", exists type = ", existsType, ", new type = ", type };
        }
    }
}

