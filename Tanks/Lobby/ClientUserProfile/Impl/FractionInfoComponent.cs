namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FractionInfoComponent : Component
    {
        public string FractionLogoImageUid { get; set; }

        public string FractionColor { get; set; }

        public string FractionName { get; set; }

        public string FractionSlogan { get; set; }

        public string FractionDescription { get; set; }

        public string FractionRewardDescription { get; set; }

        public string FractionRewardImageUid { get; set; }
    }
}

