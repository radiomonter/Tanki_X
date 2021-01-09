namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class GroupNotRegisteredException : Exception
    {
        public GroupNotRegisteredException(Type componentClass) : base(componentClass.FullName)
        {
        }
    }
}

