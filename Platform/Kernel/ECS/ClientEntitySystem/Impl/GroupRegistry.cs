namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public interface GroupRegistry
    {
        T FindOrCreateGroup<T>(long key) where T: GroupComponent;
        GroupComponent FindOrCreateGroup(Type groupClass, long key);
        GroupComponent FindOrRegisterGroup(GroupComponent groupComponent);
        T FindOrRegisterGroup<T>(T groupComponent) where T: GroupComponent;
    }
}

