namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class ComponentInfoNotFoundException : Exception
    {
        public ComponentInfoNotFoundException(Type infoType, MethodInfo componentMethod) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "infoType=", infoType, " componentMethod=", componentMethod };
        }
    }
}

