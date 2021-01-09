namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class CustomBattlesScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<ArcadeModeNode, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<ArcadeModeNode, int> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<ArcadeModeNode, int> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<ArcadeModeNode, bool> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<ArcadeModeNode, int> <>f__am$cache4;

        private bool CanShowByRestrictions(MatchMakingModeRestrictionsComponent restrictions, int userRank) => 
            (userRank <= restrictions.MaximalShowRank) && (userRank >= restrictions.MinimalShowRank);

        private void CreateActiveModeInstance(Entity mode, GameObject prefab, GameObject container)
        {
            this.CreateModeInstance(mode, prefab, container).transform.SetAsFirstSibling();
        }

        private void CreateInactiveModeInstance(Entity mode, GameObject prefab, GameObject container)
        {
            this.CreateModeInstance(mode, prefab, container).transform.SetAsLastSibling();
        }

        private GameObject CreateModeInstance(Entity mode, GameObject prefab, GameObject container)
        {
            GameObject obj2 = Object.Instantiate<GameObject>(prefab);
            obj2.transform.SetParent(container.transform, false);
            obj2.GetComponent<EntityBehaviour>().BuildEntity(mode);
            return obj2;
        }

        [OnEventFire]
        public void InitModes(NodeAddedEvent e, CustomBattlesScreenNode screen, [JoinAll] ICollection<ArcadeModeNode> modes, [JoinAll] SelfUserRankNode selfUserRank)
        {
            <InitModes>c__AnonStorey0 storey = new <InitModes>c__AnonStorey0 {
                screen = screen,
                $this = this
            };
            if (modes.Count != 0)
            {
                storey.modePrefab = storey.screen.customBattlesScreen.GameModeItemPrefab;
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = m => m.matchMakingModeActivation.Active;
                }
                <>f__am$cache1 ??= m => m.orderItem.Index;
                List<ArcadeModeNode> source = modes.Where<ArcadeModeNode>(<>f__am$cache0).OrderBy<ArcadeModeNode, int>(<>f__am$cache1).ToList<ArcadeModeNode>();
                storey.userRank = selfUserRank.userRank.Rank;
                <>f__am$cache2 ??= i => i.orderItem.Index;
                source.OrderByDescending<ArcadeModeNode, int>(<>f__am$cache2).ToList<ArcadeModeNode>().ForEach(new Action<ArcadeModeNode>(storey.<>m__0));
                <>f__am$cache3 ??= m => !m.matchMakingModeActivation.Active;
                <>f__am$cache4 ??= m => m.orderItem.Index;
                modes.Where<ArcadeModeNode>(<>f__am$cache3).OrderBy<ArcadeModeNode, int>(<>f__am$cache4).ToList<ArcadeModeNode>().ForEach(new Action<ArcadeModeNode>(storey.<>m__1));
                storey.screen.customBattlesScreen.ScrollToTheLeft();
            }
        }

        [OnEventFire]
        public void RemoveModes(NodeRemoveEvent e, CustomBattlesScreenNode screen, [JoinAll, Combine] ArcadeModeGUINode gameMode)
        {
            Object.Destroy(gameMode.gameModeSelectButton.gameObject);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <InitModes>c__AnonStorey0
        {
            internal int userRank;
            internal GameObject modePrefab;
            internal CustomBattlesScreenSystem.CustomBattlesScreenNode screen;
            internal CustomBattlesScreenSystem $this;

            internal void <>m__0(CustomBattlesScreenSystem.ArcadeModeNode mode)
            {
                if (this.$this.CanShowByRestrictions(mode.matchMakingModeRestrictions, this.userRank))
                {
                    this.$this.CreateActiveModeInstance(mode.Entity, this.modePrefab, this.screen.customBattlesScreen.GameModesContainer);
                }
            }

            internal void <>m__1(CustomBattlesScreenSystem.ArcadeModeNode mode)
            {
                if (this.$this.CanShowByRestrictions(mode.matchMakingModeRestrictions, this.userRank))
                {
                    this.$this.CreateInactiveModeInstance(mode.Entity, this.modePrefab, this.screen.customBattlesScreen.GameModesContainer);
                }
            }
        }

        public class ArcadeModeGUINode : CustomBattlesScreenSystem.ArcadeModeNode
        {
            public GameModeSelectButtonComponent gameModeSelectButton;
        }

        [Not(typeof(MatchMakingDefaultModeComponent))]
        public class ArcadeModeNode : Node
        {
            public MatchMakingArcadeModeComponent matchMakingArcadeMode;
            public MatchMakingModeComponent matchMakingMode;
            public OrderItemComponent orderItem;
            public MatchMakingModeActivationComponent matchMakingModeActivation;
            public MatchMakingModeRestrictionsComponent matchMakingModeRestrictions;
        }

        public class CustomBattlesScreenNode : Node
        {
            public CustomBattlesScreenComponent customBattlesScreen;
        }

        public class MountedHullNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;
        }

        public class SelfUserRankNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
        }
    }
}

