namespace Platform.System.Data.Statics.ClientYaml.API
{
    using System;

    public class WrongYamlStructureException : Exception
    {
        public WrongYamlStructureException(string key, Type expected, Type actual) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { key, " accepts type ", expected, ", but was ", actual };
        }
    }
}

