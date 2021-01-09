namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientEntrance.API;

    public class BattleSeriesUiSystem : ECSSystem
    {
        [OnEventFire]
        public void ComponentInit(NodeAddedEvent e, SingleNode<BattleSeriesUiComponent> ui, [JoinAll] SingleNode<BattleResultsComponent> results, [JoinAll] UserNode user)
        {
            int needGoodBattles = user.battleLeaveCounter.NeedGoodBattles;
            PersonalBattleResultForClient personalResult = results.component.ResultForClient.PersonalResult;
            if (needGoodBattles > 0)
            {
                ui.component.CurrentBattleCount = -needGoodBattles;
                ui.component.BattleSeriesPercent = -needGoodBattles;
                ui.component.ExperienceMultiplier = -needGoodBattles;
                ui.component.ContainerScoreMultiplier = 0f;
            }
            else if ((personalResult == null) || ((personalResult.MaxBattleSeries == 0) || (personalResult.CurrentBattleSeries == 0)))
            {
                ui.component.gameObject.SetActive(false);
            }
            else
            {
                float num2 = ((float) personalResult.CurrentBattleSeries) / ((float) personalResult.MaxBattleSeries);
                ui.component.BattleSeriesPercent = num2;
                ui.component.CurrentBattleCount = personalResult.CurrentBattleSeries;
                ui.component.ExperienceMultiplier = personalResult.ScoreBattleSeriesMultiplier;
                ui.component.ContainerScoreMultiplier = 0f;
            }
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public BattleLeaveCounterComponent battleLeaveCounter;
        }
    }
}

