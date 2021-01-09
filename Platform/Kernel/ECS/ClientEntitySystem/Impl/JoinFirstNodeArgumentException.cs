namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class JoinFirstNodeArgumentException : Exception
    {
        public JoinFirstNodeArgumentException(MethodInfo method, HandlerArgument handlerArgument) : base($"Join-node can''t be first node-argument of method
[method={method},
handlerArgument={handlerArgument}]")
        {
        }
    }
}

