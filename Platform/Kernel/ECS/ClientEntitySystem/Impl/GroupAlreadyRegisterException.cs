namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class GroupAlreadyRegisterException : Exception
    {
        public GroupAlreadyRegisterException(Type componentClass) : base(componentClass.FullName)
        {
        }
    }
}

