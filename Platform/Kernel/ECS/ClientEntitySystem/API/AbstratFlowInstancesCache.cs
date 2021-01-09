namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientDataStructures.Impl.Cache;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class AbstratFlowInstancesCache : FlowListener
    {
        protected List<AbstractCache> caches = new List<AbstractCache>();
        [CompilerGenerated]
        private static Action<AbstractCache> <>f__am$cache0;

        public AbstratFlowInstancesCache()
        {
            EngineService.AddFlowListener(this);
        }

        public virtual void OnFlowClean()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = c => c.FreeAll();
            }
            this.caches.ForEach(<>f__am$cache0);
        }

        public void OnFlowFinish()
        {
        }

        protected Cache<T> Register<T>()
        {
            CacheImpl<T> item = new CacheImpl<T>();
            this.caches.Add(item);
            return item;
        }

        protected Cache<T> Register<T>(Action<T> cleaner)
        {
            CacheImpl<T> item = new CacheImpl<T>(cleaner);
            this.caches.Add(item);
            return item;
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }
    }
}

