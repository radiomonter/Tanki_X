namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class NormalRPMSoundModifier : AbstractRPMSoundModifier
    {
        public override float CalculateLoadPartForModifier(float smoothedLoad) => 
            Mathf.Sqrt(1f - base.CalculateLinearLoadModifier(smoothedLoad));

        public override bool CheckLoad(float smoothedLoad) => 
            smoothedLoad < 1f;
    }
}

