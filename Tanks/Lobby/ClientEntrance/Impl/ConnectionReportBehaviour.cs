namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.Impl;
    using System;
    using UnityEngine;

    public class ConnectionReportBehaviour : MonoBehaviour
    {
        public ConnectionReportSystem system;
        public float connectionWaitTime = 30f;
        private float startTime;

        private void OnEnable()
        {
            LoggerProvider.GetLogger<ConnectionReportActivator>().Info("StartClient");
            this.startTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup > (this.startTime + this.connectionWaitTime))
            {
                if (!this.system.hasConnection)
                {
                    string message = "Client did not receive ClientSession in " + this.connectionWaitTime + " seconds. ";
                    if (InitConfiguration.Config != null)
                    {
                        string str2 = message;
                        string[] textArray1 = new string[] { str2, " InitConfig: ", InitConfiguration.Config.Host, ":", InitConfiguration.Config.AcceptorPort };
                        message = string.Concat(textArray1);
                    }
                    LoggerProvider.GetLogger<ConnectionReportActivator>().Error(message, new ClientSessionLoadTimeoutException());
                }
                Destroy(base.gameObject);
            }
            if (this.system.hasConnection)
            {
                LoggerProvider.GetLogger<ConnectionReportActivator>().Info("ClientStarted elapsed=" + (Time.realtimeSinceStartup - this.startTime));
                Destroy(base.gameObject);
            }
        }
    }
}

