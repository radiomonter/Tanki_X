namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class UnsupportedModificationException : Exception
    {
        public UnsupportedModificationException(Entity currentKey, Entity newKey) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "currentKey = ", currentKey, ", newKey = ", newKey };
        }
    }
}

