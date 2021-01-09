namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SoundLoaderWaitActivator : UnityAwareActivator<ManuallyCompleting>, ECSActivator, Activator
    {
        private bool startedLoading;

        protected override void Activate()
        {
            this.startedLoading = true;
        }

        public void RegisterSystemsAndTemplates()
        {
            EngineService.SystemRegistry.RegisterNode<LoadedSoundNode>();
        }

        private void Update()
        {
            if (this.startedLoading)
            {
                LoadedSoundNode node = EngineService.Engine.SelectAll<LoadedSoundNode>().FirstOrDefault<LoadedSoundNode>();
                if (node != null)
                {
                    SoundListenerResourcesBehaviour component = ((GameObject) node.resourceData.Data).GetComponent<SoundListenerResourcesBehaviour>();
                    node.Entity.AddComponent(new SoundListenerResourcesComponent(component));
                    base.enabled = false;
                    base.Complete();
                }
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public class LoadedSoundNode : Node
        {
            public SoundListenerComponent soundListener;
            public ResourceDataComponent resourceData;
        }
    }
}

