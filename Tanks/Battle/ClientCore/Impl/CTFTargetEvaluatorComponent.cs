namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CTFTargetEvaluatorComponent : Component
    {
        public CTFTargetEvaluatorComponent()
        {
        }

        public CTFTargetEvaluatorComponent(float flagCarrierPriorityBonus)
        {
            this.FlagCarrierPriorityBonus = flagCarrierPriorityBonus;
        }

        public float FlagCarrierPriorityBonus { get; set; }
    }
}

