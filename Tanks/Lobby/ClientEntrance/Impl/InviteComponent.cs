namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14f3b4374d5L)]
    public class InviteComponent : SharedChangeableComponent
    {
        private string inviteCode;

        public bool ShowScreenOnEntrance { get; set; }

        [ProtocolOptional]
        public string InviteCode
        {
            get => 
                this.inviteCode;
            set
            {
                this.inviteCode = value;
                base.OnChange();
            }
        }
    }
}

