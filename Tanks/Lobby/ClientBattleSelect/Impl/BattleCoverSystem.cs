namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Linq;
    using UnityEngine;

    public class BattleCoverSystem : ECSSystem
    {
        private int updateBgAtFrame = -1;

        [OnEventFire]
        public void DelayShowBackgroundAndLoadHangar(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round, [JoinAll] CoverNode cover)
        {
            base.NewEvent<ShowCoverEvent>().Attach(cover).ScheduleDelayed(3f);
        }

        [OnEventFire]
        public void HideBackground(NodeRemoveEvent e, SingleNode<BattleResultsScreenPartComponent> part)
        {
            this.UpgradeBackgroundWithDelay();
        }

        [OnEventFire]
        public void HideBackground(UpdateEvent e, CoverNode cover)
        {
            if (Time.frameCount == this.updateBgAtFrame)
            {
                this.UpgradeBackground(cover.battleScreenCover);
            }
        }

        [OnEventFire]
        public void Show(ShowCoverEvent e, CoverNode cover)
        {
            this.ShowCover(cover.battleScreenCover, true);
        }

        [OnEventFire]
        public void ShowBackground(NodeAddedEvent e, SingleNode<BattleResultsScreenPartComponent> part)
        {
            this.UpgradeBackgroundWithDelay();
        }

        private void ShowCover(BattleScreenCoverComponent cover, bool show)
        {
            cover.battleCoverAnimator.SetBool("show", show);
        }

        private void UpgradeBackground(BattleScreenCoverComponent cover)
        {
            bool show = base.SelectAll<SingleNode<BattleResultsScreenPartComponent>>().Any<SingleNode<BattleResultsScreenPartComponent>>();
            this.ShowCover(cover, show);
        }

        private void UpgradeBackgroundWithDelay()
        {
            this.updateBgAtFrame = Time.frameCount + 1;
        }

        public class CoverNode : Node
        {
            public BattleScreenCoverComponent battleScreenCover;
        }

        public class ShowCoverEvent : Event
        {
        }
    }
}

