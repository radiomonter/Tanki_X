namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System.Reflection;

    public interface HandlerFactory
    {
        Handler CreateHandler(MethodInfo method, ECSSystem system);
    }
}

