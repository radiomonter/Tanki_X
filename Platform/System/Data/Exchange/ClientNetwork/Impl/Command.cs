namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public interface Command
    {
        void Execute(Engine engine);
    }
}

