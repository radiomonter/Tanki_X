﻿namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1562b6e6ccaL)]
    public interface WeaponSkinItemTemplate : Template
    {
        [AutoAdded]
        WeaponSkinItemComponent weaponSkinItem();
    }
}

