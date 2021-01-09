namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;

    public class GoToModulesScreenEvent : Event
    {
        private Tanks.Lobby.ClientGarage.API.TankPartModuleType tankPartModuleType;

        public GoToModulesScreenEvent(Tanks.Lobby.ClientGarage.API.TankPartModuleType tankPartModuleType)
        {
            this.tankPartModuleType = tankPartModuleType;
        }

        public Tanks.Lobby.ClientGarage.API.TankPartModuleType TankPartModuleType =>
            this.tankPartModuleType;
    }
}

