namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.InteropServices;

    public interface SharedEntityRegistry
    {
        EntityInternal CreateEntity(long entityId);
        EntityInternal CreateEntity(long entityId, Optional<TemplateAccessor> templateAccessor);
        EntityInternal CreateEntity(long templateId, string configPath, long entityId);
        bool IsShared(long entityId);
        void SetShared(long entityId);
        void SetUnshared(long entityId);
        bool TryGetEntity(long entityId, out EntityInternal entity);
    }
}

