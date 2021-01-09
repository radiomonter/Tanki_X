namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;

    public class EnergyBonusUISystem : ECSSystem
    {
        [OnEventFire]
        public void SetBonusActive(NodeRemoveEvent e, SingleNode<TakenBonusComponent> userItem, [JoinAll] SingleNode<EnergyBonusContent> energyBonusContent)
        {
            energyBonusContent.component.SetBonusActive();
        }

        [OnEventFire]
        public void SetBonusInactive(NodeAddedEvent e, TakenEnergyBonusNode userItem, [JoinAll] SingleNode<EnergyBonusContent> energyBonusContent)
        {
            energyBonusContent.component.SetBonusInactive();
        }

        public class TakenEnergyBonusNode : Node
        {
            public UserGroupComponent userGroup;
            public EnergyBonusComponent energyBonus;
            public ExpireDateComponent expireDate;
            public TakenBonusComponent takenBonus;
        }
    }
}

