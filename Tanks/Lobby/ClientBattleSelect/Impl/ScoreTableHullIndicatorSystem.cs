namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientGarage.API;

    public class ScoreTableHullIndicatorSystem : ECSSystem
    {
        private void SetHull(HullIndicatorNode hullIndicator, HullNode hull)
        {
            hullIndicator.scoreTableHullIndicator.SetHullIcon(hull.marketItemGroup.Key);
        }

        [OnEventFire]
        public void SetHulls(NodeAddedEvent e, [Combine] HullIndicatorNode hullIndicator, [Context, JoinByUser] HullNode hull)
        {
            this.SetHull(hullIndicator, hull);
        }

        public class HullIndicatorNode : Node
        {
            public ScoreTableHullIndicatorComponent scoreTableHullIndicator;
            public UserGroupComponent userGroup;
        }

        public class HullNode : Node
        {
            public UserGroupComponent userGroup;
            public TankComponent tank;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

