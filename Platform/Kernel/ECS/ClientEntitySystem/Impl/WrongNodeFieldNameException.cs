namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class WrongNodeFieldNameException : Exception
    {
        public WrongNodeFieldNameException(Type nodeClass, Type fieldType, string fieldName) : base($"node = {nodeClass}, fieldType = {fieldType.Name}, fieldName = {fieldName}")
        {
        }
    }
}

