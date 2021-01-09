namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class InputManagerSystem : ECSSystem
    {
        [OnEventFire]
        public void ClearInputManager(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> user)
        {
            InputManager.ClearActions();
        }

        [OnEventFire]
        public void ClearInputManager(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> user)
        {
            InputManager.ClearActions();
        }

        [OnEventFire]
        public void ClearInputManager(ApplicationFocusEvent e, SingleNode<SelfUserComponent> user)
        {
            if (!e.IsFocused)
            {
                InputManager.ClearActions();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

