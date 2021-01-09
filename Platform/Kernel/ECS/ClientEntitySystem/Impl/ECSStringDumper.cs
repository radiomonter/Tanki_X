namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class ECSStringDumper
    {
        public static string Build(bool detailInfo = false)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Entity entity in (EngineService == null) ? new List<Entity>() : EngineService.EntityRegistry.GetAllEntities())
            {
                if (!EngineService.EntityStub.Equals(entity))
                {
                    builder.Append($"[Entity: Id={entity.Id}, Name={entity.Name}]
");
                    EntityInternal internal2 = (EntityInternal) entity;
                    foreach (Component component in !(internal2 is EntityStub) ? internal2.Components : new List<Component>())
                    {
                        builder.Append("[Component: ");
                        builder.Append(!detailInfo ? component.GetType().Name : EcsToStringUtil.ToStringWithProperties(component, ", "));
                        builder.Append("]\n");
                    }
                }
            }
            return builder.ToString();
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

