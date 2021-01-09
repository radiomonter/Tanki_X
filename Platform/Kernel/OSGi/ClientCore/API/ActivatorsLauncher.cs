namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ActivatorsLauncher
    {
        private readonly Queue<Activator> activators;

        public ActivatorsLauncher(IEnumerable<Activator> activators)
        {
            this.activators = new Queue<Activator>(activators);
        }

        private void LaunchActivator(Action onComplete = null)
        {
            <LaunchActivator>c__AnonStorey0 storey = new <LaunchActivator>c__AnonStorey0 {
                onComplete = onComplete,
                $this = this
            };
            if (this.activators.Count > 0)
            {
                this.CurrentActivator = this.activators.Dequeue();
                this.CurrentActivator.Launch(new Action(storey.<>m__0));
            }
            else if (storey.onComplete != null)
            {
                storey.onComplete();
            }
        }

        public void LaunchAll(Action onComplete = null)
        {
            this.LaunchActivator(onComplete);
        }

        public Activator CurrentActivator { get; private set; }

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

