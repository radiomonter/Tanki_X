namespace Tanks.ClientLauncher
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ActivatorsLauncher
    {
        private readonly ILog logger;
        private readonly Queue<Activator> activators;

        public ActivatorsLauncher(IEnumerable<Activator> activators)
        {
            this.activators = new Queue<Activator>(activators);
            this.logger = LoggerProvider.GetLogger<ActivatorsLauncher>();
        }

        public void LaunchActivator(Action onComplete = null)
        {
            <LaunchActivator>c__AnonStorey0 storey = new <LaunchActivator>c__AnonStorey0 {
                onComplete = onComplete,
                $this = this
            };
            if (this.activators.Count > 0)
            {
                Activator activator = this.activators.Dequeue();
                this.logger.InfoFormat("Activate {0}", activator.GetType());
                activator.Launch(new Action(storey.<>m__0));
            }
            else if (storey.onComplete != null)
            {
                storey.onComplete();
            }
        }

        public void LaunchAll(Action onComplete = null)
        {
            InjectionUtils.RegisterInjectionPoints(typeof(InjectAttribute), ServiceRegistry.Current);
            this.LaunchECSActivators();
            this.LaunchActivator(onComplete);
        }

        private void LaunchECSActivators()
        {
            foreach (Activator activator in this.activators)
            {
                if (activator is ECSActivator)
                {
                    this.logger.InfoFormat("Activate ECS part {0}", activator.GetType());
                    ((ECSActivator) activator).RegisterSystemsAndTemplates();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LaunchActivator>c__AnonStorey0
        {
            internal Action onComplete;
            internal ActivatorsLauncher $this;

            internal void <>m__0()
            {
                this.$this.LaunchActivator(this.onComplete);
            }
        }
    }
}

