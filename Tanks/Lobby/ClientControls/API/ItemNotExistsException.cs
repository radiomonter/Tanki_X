namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ItemNotExistsException : ArgumentException
    {
        public ItemNotExistsException(Entity entity) : base("Item with entity = " + entity.Name + " not exists")
        {
        }
    }
}

