namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class ConnectionReportActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public float connectionWaitTime = 15f;
        private ConnectionReportSystem system;

        protected override void Activate()
        {
            GameObject target = new GameObject("ConnectionReport");
            DontDestroyOnLoad(target);
            target.AddComponent<SkipRemoveOnSceneSwitch>();
            ConnectionReportBehaviour behaviour = target.AddComponent<ConnectionReportBehaviour>();
            behaviour.system = this.system;
            behaviour.connectionWaitTime = this.connectionWaitTime;
        }

        public void RegisterSystemsAndTemplates()
        {
            this.system = new ConnectionReportSystem();
            ECSBehaviour.EngineService.RegisterSystem(this.system);
        }
    }
}

