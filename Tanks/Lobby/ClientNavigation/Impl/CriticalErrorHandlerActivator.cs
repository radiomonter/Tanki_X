namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CriticalErrorHandlerActivator : UnityAwareActivator<AutoCompleting>
    {
        [CompilerGenerated]
        private static Application.LogCallback <>f__mg$cache0;

        protected override void Activate()
        {
            <>f__mg$cache0 ??= new Application.LogCallback(FatalErrorHandler.HandleLog);
            Application.logMessageReceivedThreaded += <>f__mg$cache0;
        }
    }
}

