namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e510bca5eL)]
    public class ExpireDateComponent : Component
    {
        public Platform.Library.ClientUnityIntegration.API.Date Date { get; set; }
    }
}

