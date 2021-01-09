namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class EnergyBonusSystem : ECSSystem
    {
        [OnEventFire]
        public void CountAvailableBonusEnergy(TryUseBonusEvent e, EnergyBonusNode bonus1, [JoinByUser] UserNode user, [JoinByLeague] LeagueNode league, EnergyBonusNode bonus2, [JoinByUser] EnergyUserItemNode energy)
        {
            e.AvailableBonusEnergy = league.leagueEnergyConfig.Capacity - energy.userItemCounter.Count;
        }

        [OnEventComplete]
        public void TryUserBonus(TryUseBonusEvent e, EnergyBonusNode bonus, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            if (e.AvailableBonusEnergy >= bonus.energyBonus.Bonus)
            {
                base.ScheduleEvent<UseBonusEvent>(bonus);
            }
            else if (e.AvailableBonusEnergy <= 0L)
            {
                dialogs.component.Get<FullEnergyDialog>().Show();
            }
            else
            {
                CantUseAllEnergyBonusDialog dialog2 = dialogs.component.Get<CantUseAllEnergyBonusDialog>();
                dialog2.SetEnergyCount(e.AvailableBonusEnergy);
                dialog2.Show(new List<Animator>());
            }
        }

        [OnEventFire]
        public void UsePartOfEnergyBonus(DialogConfirmEvent e, SingleNode<CantUseAllEnergyBonusDialog> dialog, [JoinAll] UserNode user, [JoinByUser] EnergyBonusNode bonus)
        {
            base.ScheduleEvent<UseBonusEvent>(bonus);
        }

        [Not(typeof(TakenBonusComponent))]
        public class EnergyBonusNode : Node
        {
            public UserGroupComponent userGroup;
            public EnergyBonusComponent energyBonus;
        }

        public class EnergyUserItemNode : Node
        {
            public UserGroupComponent userGroup;
            public EnergyItemComponent energyItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        public class LeagueNode : Node
        {
            public LeagueComponent league;
            public LeagueGroupComponent leagueGroup;
            public LeagueEnergyConfigComponent leagueEnergyConfig;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public LeagueGroupComponent leagueGroup;
        }
    }
}

