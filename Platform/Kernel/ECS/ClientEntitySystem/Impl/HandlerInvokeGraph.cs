namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class HandlerInvokeGraph
    {
        public HandlerInvokeGraph(Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler handler)
        {
            this.Handler = handler;
            IList<HandlerArgument> handlerArguments = handler.HandlerArgumentsDescription.HandlerArguments;
            this.ArgumentNodes = new ArgumentNode[handlerArguments.Count];
            for (int i = 0; i < handlerArguments.Count; i++)
            {
                this.ArgumentNodes[i] = new ArgumentNode(handlerArguments[i]);
            }
        }

        public void Clear()
        {
            IList<HandlerArgument> handlerArguments = this.Handler.HandlerArgumentsDescription.HandlerArguments;
            for (int i = 0; i < handlerArguments.Count; i++)
            {
                this.ArgumentNodes[i].Clear();
            }
        }

        public HandlerInvokeGraph Init()
        {
            this.Clear();
            return this;
        }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler Handler { get; private set; }

        public ArgumentNode[] ArgumentNodes { get; private set; }
    }
}

