namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class HandlerFactoryException : Exception
    {
        public HandlerFactoryException(MethodInfo method) : base("Method: " + method)
        {
        }

        public HandlerFactoryException(MethodInfo method, Type type) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "Method: ", method, ", type: ", type };
        }
    }
}

