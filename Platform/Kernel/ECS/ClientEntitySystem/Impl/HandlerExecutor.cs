namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public class HandlerExecutor
    {
        public HandlerExecutor()
        {
        }

        public HandlerExecutor(Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler handler, object[] argumentForMethod)
        {
            this.Handler = handler;
            this.ArgumentForMethod = argumentForMethod;
        }

        public virtual void Execute()
        {
            this.Handler.Invoke(this.ArgumentForMethod);
        }

        public HandlerExecutor Init(Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler handler, object[] argumentForMethod)
        {
            this.Handler = handler;
            this.ArgumentForMethod = argumentForMethod;
            return this;
        }

        public void SetEvent(object eventInstance)
        {
            this.ArgumentForMethod[0] = eventInstance;
        }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler Handler { get; private set; }

        public object[] ArgumentForMethod { get; private set; }
    }
}

