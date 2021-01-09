namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class TwinsEnergyBarSystem : ECSSystem
    {
        [OnEventComplete]
        public void Init(NodeAddedEvent e, TwinsWeaponNode weapon, [JoinByTank, Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.EnergyBarEnabled = false;
        }

        public class TwinsWeaponNode : Node
        {
            public TwinsComponent twins;
        }
    }
}

