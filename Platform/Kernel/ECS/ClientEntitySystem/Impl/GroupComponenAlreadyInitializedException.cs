namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class GroupComponenAlreadyInitializedException : Exception
    {
        public GroupComponenAlreadyInitializedException(GroupComponent groupComponent) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "componentClass=", groupComponent.GetType().FullName, ", key=", groupComponent.Key };
        }
    }
}

