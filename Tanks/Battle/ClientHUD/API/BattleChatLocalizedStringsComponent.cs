namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BattleChatLocalizedStringsComponent : Component
    {
        public string TeamChatInputHint { get; set; }

        public string GeneralChatInputHint { get; set; }
    }
}

