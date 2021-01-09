namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class GroupComponentKeyNotInitializedException : Exception
    {
        public GroupComponentKeyNotInitializedException(GroupComponent groupComponent) : base("groupComponent=" + groupComponent.GetType())
        {
        }
    }
}

