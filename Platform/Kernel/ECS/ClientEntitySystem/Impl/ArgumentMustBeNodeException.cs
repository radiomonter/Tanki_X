namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class ArgumentMustBeNodeException : Exception
    {
        public ArgumentMustBeNodeException(MethodInfo method, ParameterInfo parameterInfo) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "method=", method, ",type=", parameterInfo.ParameterType };
        }
    }
}

