﻿namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;

    public interface IAvatarStateChanger
    {
        Action<bool> SetSelected { get; set; }

        Action<bool> SetEquipped { get; set; }

        Action<bool> SetUnlocked { get; set; }

        Action OnBought { get; set; }
    }
}

