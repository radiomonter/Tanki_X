namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using Tanks.Battle.ClientCore.API;

    public class BattleResultStatScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void ShowMatchDetails(NodeAddedEvent e, SingleNode<BattleResultsScreenStatComponent> screen, [JoinAll] ResultsNode results)
        {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            string name = base.GetEntityById(resultForClient.MapId).GetComponent<DescriptionItemComponent>().Name;
            BattleMode battleMode = resultForClient.BattleMode;
            string str2 = $"{battleMode}, {name}";
            screen.component.BattleDescription = str2;
            if (results.battleResults.ResultForClient.BattleMode == BattleMode.DM)
            {
                screen.component.ShowDMMatchDetails();
            }
            else
            {
                screen.component.ShowTeamMatchDetails();
            }
        }

        public class ResultsNode : Node
        {
            public BattleResultsComponent battleResults;
        }
    }
}

