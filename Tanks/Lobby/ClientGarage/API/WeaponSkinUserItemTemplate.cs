﻿namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x1562b72c164L)]
    public interface WeaponSkinUserItemTemplate : WeaponSkinItemTemplate, SkinUserItemTemplate, Template, SkinItemTemplate, UserItemTemplate, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate
    {
    }
}

