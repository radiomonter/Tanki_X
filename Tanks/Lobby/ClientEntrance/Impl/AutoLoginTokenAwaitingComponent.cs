namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AutoLoginTokenAwaitingComponent : Component
    {
        public AutoLoginTokenAwaitingComponent()
        {
        }

        public AutoLoginTokenAwaitingComponent(byte[] passwordDigest)
        {
            this.PasswordDigest = passwordDigest;
        }

        public byte[] PasswordDigest { get; set; }
    }
}

