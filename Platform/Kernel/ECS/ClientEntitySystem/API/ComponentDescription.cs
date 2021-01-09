namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface ComponentDescription
    {
        T GetInfo<T>() where T: ComponentInfo;
        bool IsInfoPresent(Type type);

        string FieldName { get; }

        Type ComponentType { get; }
    }
}

