namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;

    public class TabSystem : ECSSystem
    {
        [OnEventComplete]
        public void OnTabPressed(UpdateEvent evt, SingleNode<TabListenerComponent> node, [JoinAll] Optional<SingleNode<RoundActiveStateComponent>> round)
        {
            bool flag = node.Entity.HasComponent<TabPressedComponent>();
            if (InputManager.CheckAction(BattleActions.SHOW_SCORE) && (!flag && round.IsPresent()))
            {
                TabPressedComponent component = new TabPressedComponent();
                node.Entity.AddComponent(component);
            }
            else if (((!InputManager.CheckAction(BattleActions.SHOW_SCORE) || !round.IsPresent()) && flag) && node.Entity.HasComponent<TabPressedComponent>())
            {
                node.Entity.RemoveComponent<TabPressedComponent>();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

