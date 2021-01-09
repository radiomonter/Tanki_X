namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ScoreTableEmptyRowIndicatorSystem : ECSSystem
    {
        [OnEventFire]
        public void Localize(NodeAddedEvent e, SingleNode<ScoreTableEmptyRowIndicatorComponent> indicator, SingleNode<ScoreTableEmptyRowTextComponent> text)
        {
            indicator.component.Text = text.component.Text;
        }
    }
}

