namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x157d29a0c49L)]
    public class QuestExpireDateComponent : Component
    {
        public Platform.Library.ClientUnityIntegration.API.Date Date { get; set; }
    }
}

