namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public interface EntityTest : Entity
    {
        void AddComponentInTest<RealT>(Component component) where RealT: Component;
        T GetComponentInTest<T>() where T: Component;
        bool HasComponentInTest<T>() where T: Component;
        void UpdateNodes();
    }
}

