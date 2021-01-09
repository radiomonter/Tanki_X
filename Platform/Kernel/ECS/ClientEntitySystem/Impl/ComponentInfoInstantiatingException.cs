namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class ComponentInfoInstantiatingException : Exception
    {
        public ComponentInfoInstantiatingException(Type componentInfoClass, Exception e) : base(string.Concat(objArray1), e)
        {
            object[] objArray1 = new object[] { "component info class = ", componentInfoClass, ", message = ", e.Message };
        }
    }
}

