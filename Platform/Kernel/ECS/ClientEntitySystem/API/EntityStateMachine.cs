namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface EntityStateMachine
    {
        void AddState<T>() where T: Node, new();
        void AttachToEntity(Entity entity);
        T ChangeState<T>() where T: Node;
        Node ChangeState(Type t);
    }
}

