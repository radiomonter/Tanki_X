namespace Platform.System.Data.Statics.ClientYaml.API
{
    using System;

    public class MergingYamlMismatchException : Exception
    {
        public MergingYamlMismatchException(string key, Type destinationType, Type sourceType) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "Destination yaml has key ", key, " with value's type ", destinationType, ", but merging yaml has this key with value's type ", sourceType };
        }
    }
}

