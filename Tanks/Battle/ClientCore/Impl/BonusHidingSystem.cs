namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class BonusHidingSystem : ECSSystem
    {
        public const float HIDING_DURATION = 1f;

        [OnEventFire]
        public void CreateBonusHidingEffectEntity(HideBonusEvent e, BonusBoxNode bonusBox)
        {
            Entity entity = base.CreateEntity("BonusHiding");
            entity.AddComponent<BonusRoundEndStateComponent>();
            BonusBoxInstanceComponent component = new BonusBoxInstanceComponent {
                BonusBoxInstance = bonusBox.bonusBoxInstance.BonusBoxInstance
            };
            entity.AddComponent(component);
            bonusBox.bonusBoxInstance.Removed = true;
            entity.AddComponent(new LocalDurationComponent(1f));
        }

        [OnEventFire]
        public void DestroyBonusBox(LocalDurationExpireEvent e, BonusBoxHidingNode hidingBonus)
        {
            hidingBonus.bonusBoxInstance.BonusBoxInstance.RecycleObject();
            base.DeleteEntity(hidingBonus.Entity);
        }

        [OnEventComplete]
        public void HideParachute(HideBonusEvent e, SingleNode<BonusParachuteInstanceComponent> bonusParachute)
        {
            bonusParachute.component.BonusParachuteInstance.RecycleObject();
            bonusParachute.Entity.RemoveComponent<BonusParachuteInstanceComponent>();
        }

        [OnEventFire]
        public void HidingProcess(UpdateEvent e, BonusBoxHidingNode bonus)
        {
            if (bonus.bonusBoxInstance.BonusBoxInstance)
            {
                float x = 1f - Date.Now.GetProgress(bonus.localDuration.StartedTime, (float) 1f);
                Vector3 vector = new Vector3(x, x, x);
                bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = vector;
            }
        }

        public class BonusBoxHidingNode : Node
        {
            public LocalDurationComponent localDuration;
            public BonusRoundEndStateComponent bonusRoundEndState;
            public BonusBoxInstanceComponent bonusBoxInstance;
        }

        public class BonusBoxNode : Node
        {
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BattleGroupComponent battleGroup;
        }
    }
}

