namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FlowHandlerInvokeDate : HandlerInvokeData
    {
        protected override HandlerExecutor CreateExecutor()
        {
            object[] instanceArray = Cache.array.GetInstanceArray(base.HandlerArguments.Count + 1);
            return Cache.handlerExecutor.GetInstance().Init(base.Handler, instanceArray);
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }
    }
}

