namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class CanNotRemoveGroupComponentFromRootGroupEntityForNotEmptyGroupException : Exception
    {
        public CanNotRemoveGroupComponentFromRootGroupEntityForNotEmptyGroupException(Type groupClass, Entity entity) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "group=", groupClass.FullName, " entity=", entity };
        }
    }
}

