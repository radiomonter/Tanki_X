namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class GraphicsSettingsBuilderGroupComponent : GroupComponent
    {
        public GraphicsSettingsBuilderGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public GraphicsSettingsBuilderGroupComponent(long key) : base(key)
        {
        }
    }
}

