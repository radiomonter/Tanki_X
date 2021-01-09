namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class ClientActivator : MonoBehaviour
    {
        private List<Activator> coreActivators;
        private List<Activator> nonCoreActivators;
        protected ActivatorsLauncher activatorsLauncher;
        [CompilerGenerated]
        private static Func<Activator, ECSActivator> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<ECSActivator, bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Action<ECSActivator> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<Activator, bool> <>f__am$cache3;

        protected ClientActivator()
        {
        }

        public void ActivateClient(List<Activator> coreActivators, List<Activator> nonCoreActivators)
        {
            this.coreActivators = coreActivators;
            this.nonCoreActivators = nonCoreActivators;
            InjectionUtils.RegisterInjectionPoints(typeof(InjectAttribute), ServiceRegistry.Current);
            this.LaunchCoreActivators();
        }

        protected IEnumerable<Activator> GetActivatorsAddedInUnityEditor()
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = a => ((MonoBehaviour) a).enabled;
            }
            return base.gameObject.GetComponentsInChildren<Activator>().Where<Activator>(<>f__am$cache3);
        }

        private void LaunchCoreActivators()
        {
            this.activatorsLauncher = new ActivatorsLauncher(this.coreActivators);
            this.activatorsLauncher.LaunchAll(new Action(this.OnCoreActivatorsLaunched));
        }

        private void OnAllActivatorsLaunched()
        {
            this.AllActivatorsLaunched = true;
            Engine engine = EngineServiceInternal.Engine;
            engine.ScheduleEvent<ClientStartEvent>(engine.CreateEntity("loader"));
        }

        private void OnCoreActivatorsLaunched()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = a => a as ECSActivator;
            }
            <>f__am$cache1 ??= a => !ReferenceEquals(a, null);
            <>f__am$cache2 ??= a => a.RegisterSystemsAndTemplates();
            this.nonCoreActivators.Select<Activator, ECSActivator>(<>f__am$cache0).Where<ECSActivator>(<>f__am$cache1).ForEach<ECSActivator>(<>f__am$cache2);
            EngineServiceInternal.RunECSKernel();
            base.gameObject.AddComponent<PreciseTimeBehaviour>();
            base.gameObject.AddComponent<EngineBehaviour>();
            this.activatorsLauncher = new ActivatorsLauncher(this.nonCoreActivators);
            this.activatorsLauncher.LaunchAll(new Action(this.OnAllActivatorsLaunched));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineServiceInternal EngineServiceInternal { get; set; }

        public bool AllActivatorsLaunched { get; private set; }
    }
}

