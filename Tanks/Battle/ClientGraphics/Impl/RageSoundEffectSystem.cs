namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class RageSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayRageEffect(TriggerEffectExecuteEvent e, SingleNode<RageEffectComponent> effect, [JoinByTank] SelfTankNode tank, [JoinByTank] ICollection<SingleNode<InventoryCooldownStateComponent>> inventorySlots)
        {
            if (inventorySlots.Count != 0)
            {
                RageSoundEffectBehaviour.CreateRageSound(tank.rageSoundEffect.Asset);
            }
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public RageSoundEffectComponent rageSoundEffect;
        }
    }
}

