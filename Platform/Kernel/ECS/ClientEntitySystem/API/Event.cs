namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;

    public class Event
    {
        public override string ToString() => 
            EcsToStringUtil.ToStringWithProperties(this, ", ");
    }
}

