namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class HUDNodes
    {
        public class ActiveSelfTankNode : HUDNodes.SelfTankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public abstract class BaseWeaponNode : Node
        {
            public WeaponComponent weapon;
            public WeaponEnergyComponent weaponEnergy;
            public TankGroupComponent tankGroup;

            protected BaseWeaponNode()
            {
            }
        }

        public class BattleUserNode : Node
        {
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
        }

        public class DeadSelfTankNode : HUDNodes.SelfTankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class Modules
        {
            public class SlotNode : Node
            {
                public SlotUserItemInfoComponent slotUserItemInfo;
                public TankGroupComponent tankGroup;
            }

            public class SlotWithModuleNode : HUDNodes.Modules.SlotNode
            {
                public ModuleGroupComponent moduleGroup;
            }
        }

        public class SelfBattleUserAsSpectatorNode : HUDNodes.SelfBattleUserNode
        {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class SelfBattleUserAsTankNode : HUDNodes.SelfBattleUserNode
        {
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class SelfBattleUserNode : HUDNodes.BattleUserNode
        {
            public SelfBattleUserComponent selfBattleUser;
        }

        public class SelfTankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public HealthComponent health;
            public HealthConfigComponent healthConfig;
            public SelfTankComponent selfTank;
        }

        public class SemiActiveSelfTankNode : HUDNodes.SelfTankNode
        {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }
    }
}

