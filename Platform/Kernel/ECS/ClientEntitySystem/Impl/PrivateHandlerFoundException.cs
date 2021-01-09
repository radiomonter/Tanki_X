namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class PrivateHandlerFoundException : Exception
    {
        public PrivateHandlerFoundException(MethodInfo method) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "method = ", method, ", class=", method.DeclaringType };
        }
    }
}

