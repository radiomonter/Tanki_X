﻿namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class UserNotificatorHUDTextComponent : Component
    {
        public string UserRankMessageFormat { get; set; }
    }
}

