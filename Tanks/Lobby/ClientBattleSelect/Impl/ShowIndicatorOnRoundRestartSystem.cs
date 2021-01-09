namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ShowIndicatorOnRoundRestartSystem : ECSSystem
    {
        [OnEventFire]
        public void Hide(NodeAddedEvent e, [Combine] SingleNode<RoundActiveStateComponent> restartingRound, [Context, Combine] SingleNode<ShowIndicatorOnRoundRestartComponent> indicator)
        {
            indicator.component.Hide();
        }

        [OnEventFire]
        public void Show(NodeAddedEvent e, [Combine] SingleNode<RoundRestartingStateComponent> restartingRound, [Context, Combine] SingleNode<ShowIndicatorOnRoundRestartComponent> indicator)
        {
            indicator.component.Show();
        }
    }
}

