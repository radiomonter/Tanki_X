namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BattleChatFocusCheckEvent : Event
    {
        public bool InputIsFocused { get; set; }
    }
}

