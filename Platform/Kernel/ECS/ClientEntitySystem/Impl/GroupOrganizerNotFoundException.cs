namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class GroupOrganizerNotFoundException : Exception
    {
        public GroupOrganizerNotFoundException(Type componentClass) : base(componentClass.FullName)
        {
        }
    }
}

