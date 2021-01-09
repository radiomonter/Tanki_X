namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class SelfDestructionControllerSystem : ECSSystem
    {
        [OnEventComplete]
        public void OnUpdate(UpdateEvent evt, SuicideControllerNode node)
        {
            if (InputManager.CheckAction(SuicideActions.SUICIDE))
            {
                node.Entity.AddComponent<SelfDestructionComponent>();
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [Not(typeof(SelfDestructionComponent))]
        public class SuicideControllerNode : Node
        {
            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;
        }
    }
}

