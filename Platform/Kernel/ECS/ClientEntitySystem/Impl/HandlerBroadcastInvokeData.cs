namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HandlerBroadcastInvokeData : HandlerInvokeData
    {
        private readonly Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity;

        public HandlerBroadcastInvokeData(Handler handler, Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity) : base(handler)
        {
            this.entity = entity;
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity =>
            this.entity;
    }
}

