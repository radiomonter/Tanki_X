﻿namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ebe74dabfL)]
    public interface RicochetItemTemplate : WeaponItemTemplate, GarageItemTemplate, Template
    {
    }
}

