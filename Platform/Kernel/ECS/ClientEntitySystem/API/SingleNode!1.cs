namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;

    public class SingleNode<T> : AbstractSingleNode where T: Component
    {
        public T component;
    }
}

