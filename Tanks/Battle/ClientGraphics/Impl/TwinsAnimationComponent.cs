namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class TwinsAnimationComponent : AbstractShotAnimationComponent
    {
        [SerializeField]
        private string[] twinsAnimationsNames = new string[] { "shotLeft", "shotRight" };
        [SerializeField]
        private string[] twinsShotSpeedCoeffNames = new string[] { "shotLeftSpeedCoeff", "shotRightSpeedCoeff" };
        [SerializeField]
        private AnimationClip[] twinsShotClips;
        private Animator twinsAnimator;
        private int[] twinsAnimationIDArray;
        private int[] twinsSpeedCoeffIDArray;

        private void CalculateSpeedCoeffs(float optimalAnimationTime, int clipCount)
        {
            for (int i = 0; i < clipCount; i++)
            {
                AnimationClip shotAnimationClip = this.twinsShotClips[i];
                float num2 = base.CalculateShotSpeedCoeff(shotAnimationClip, optimalAnimationTime, false);
                this.twinsAnimator.SetFloat(this.twinsSpeedCoeffIDArray[i], num2);
            }
        }

        private void ConvertParamsFromString2ID(string[] strArray, ref int[] IDArray)
        {
            int length = strArray.Length;
            IDArray = new int[length];
            for (int i = 0; i < length; i++)
            {
                IDArray[i] = Animator.StringToHash(strArray[i]);
            }
        }

        public void Init(Animator animator, float cooldownTimeSec, float eShot, float energyReloadingSpeed)
        {
            this.twinsAnimator = animator;
            this.ConvertParamsFromString2ID(this.twinsAnimationsNames, ref this.twinsAnimationIDArray);
            this.ConvertParamsFromString2ID(this.twinsShotSpeedCoeffNames, ref this.twinsSpeedCoeffIDArray);
            int length = this.twinsShotClips.Length;
            float optimalAnimationTime = base.CalculateOptimalAnimationTime(energyReloadingSpeed, cooldownTimeSec, eShot) * length;
            this.CalculateSpeedCoeffs(optimalAnimationTime, length);
        }

        public void Play(int index)
        {
            this.twinsAnimator.SetTrigger(this.twinsAnimationIDArray[index]);
        }
    }
}

