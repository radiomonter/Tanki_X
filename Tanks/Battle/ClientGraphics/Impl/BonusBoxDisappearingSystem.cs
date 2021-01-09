namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;

    public class BonusBoxDisappearingSystem : ECSSystem
    {
        [OnEventFire]
        public void RemoveBrokenBox(LocalDurationExpireEvent e, TakenBrokenBonusBoxNode bonus)
        {
            bonus.brokenBonusBoxInstance.Instance.RecycleObject();
            base.DeleteEntity(bonus.Entity);
        }

        [OnEventFire]
        public void UpdateBrokenBonusBoxAlpha(TimeUpdateEvent e, TakenBrokenBonusBoxNode node)
        {
            float progress = Date.Now.GetProgress(node.localDuration.StartedTime, node.localDuration.Duration);
            float alpha = 1f - ((progress >= 0.9f) ? ((progress - 0.9f) / 0.1f) : 0f);
            node.materialArray.Materials.SetAlpha(alpha);
        }

        public class TakenBrokenBonusBoxNode : Node
        {
            public BonusTakingStateComponent bonusTakingState;
            public MaterialArrayComponent materialArray;
            public BrokenBonusBoxInstanceComponent brokenBonusBoxInstance;
            public LocalDurationComponent localDuration;
        }
    }
}

