namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class GroupComponentNotOnEntityException : Exception
    {
        public GroupComponentNotOnEntityException(Type componentType) : base("componentType=" + componentType)
        {
        }
    }
}

