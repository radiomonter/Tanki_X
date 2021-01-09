namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SelfUserComponent : Component, AttachToEntityListener
    {
        public void AttachedToEntity(Entity entity)
        {
            SelfUser = entity;
        }

        public static Entity SelfUser { get; set; }
    }
}

