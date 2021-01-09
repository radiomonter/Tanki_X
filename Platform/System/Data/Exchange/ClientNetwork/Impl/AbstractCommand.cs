namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public abstract class AbstractCommand : Command
    {
        protected AbstractCommand()
        {
        }

        public abstract void Execute(Engine engine);
    }
}

