namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16054e30a57L)]
    public class PremiumAccountQuestComponent : Component
    {
        public Date EndDate { get; set; }
    }
}

