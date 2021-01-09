namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BonusParachuteDisappearingSystem : ECSSystem
    {
        [OnEventFire]
        public void FoldParachute(TimeUpdateEvent e, SeparatedParachuteNode parachute)
        {
            if (parachute.bonusParachuteInstance.BonusParachuteInstance)
            {
                float progress = Date.Now.GetProgress(parachute.localDuration.StartedTime, parachute.localDuration.Duration);
                float x = 1f - (progress * ((1f - parachute.separateParachute.parachuteFoldingScaleByXZ) / parachute.localDuration.Duration));
                parachute.bonusParachuteInstance.BonusParachuteInstance.transform.localScale = new Vector3(x, 1f - (progress * ((1f - parachute.separateParachute.parachuteFoldingScaleByY) / parachute.localDuration.Duration)), x);
                float alpha = 1f - progress;
                parachute.parachuteMaterial.Material.SetAlpha(alpha);
            }
        }

        [OnEventFire]
        public void RemoveParachute(LocalDurationExpireEvent e, SeparatedParachuteNode bonus)
        {
            bonus.bonusParachuteInstance.BonusParachuteInstance.RecycleObject();
            base.DeleteEntity(bonus.Entity);
        }

        public class SeparatedParachuteNode : Node
        {
            public SeparateParachuteComponent separateParachute;
            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public ParachuteMaterialComponent parachuteMaterial;
            public LocalDurationComponent localDuration;
        }
    }
}

