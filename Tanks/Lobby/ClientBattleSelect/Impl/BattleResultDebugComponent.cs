namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine.EventSystems;

    public class BattleResultDebugComponent : UIBehaviour, Component
    {
        public double Reputation = 2100.0;
        public double ReputationDelta = 10.0;
        public int Energy = 10;
        public int EnergyDelta = 5;
        public int CrystalsForExtraEnergy = 0x3e8;
        public EnergySource MaxEnergySource = EnergySource.MVP_BONUS;
        public int RankExp = 800;
        public int RankExpDelta = 100;
        public int WeaponExp = 0x1f40;
        public int WeaponInitExp = 500;
        public int WeaponFinalExp = 0x7d0;
        public int TankExp = 0x1770;
        public int TankInitExp = 0x1388;
        public int TankFinalExp = 0x2710;
        public int ItemsExpDelta = 0x3e8;
        public int ContainerScore = 50;
        public float ContainerScoreMultiplier = 2f;
        public int ContainerScoreDelta = 300;
        public int ContainerScoreLimit = 100;
        public long Reward = 0x10000003eL;
        public long HullId = 0x200de55dL;
        public long WeaponId = 0x3cdc0dabL;
        public long HullSkinId = -1194388226L;
        public long WeaponSkinId = -472765007L;
    }
}

