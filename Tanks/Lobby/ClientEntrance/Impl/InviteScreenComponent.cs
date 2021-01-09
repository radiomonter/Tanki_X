namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;

    [SerialVersionUID(0x8d2e6e0d99fd73aL)]
    public class InviteScreenComponent : BehaviourComponent, NoScaleScreen
    {
        public InputFieldComponent InviteField;

        public InviteScreenComponent(InputFieldComponent inviteField)
        {
            this.InviteField = inviteField;
        }
    }
}

