namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x159365319aaL)]
    public class UserIncompleteRegistrationComponent : Component
    {
        public bool FirstBattleDone { get; set; }
    }
}

