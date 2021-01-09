namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public abstract class DamageAnimationStateMachineBehaviour : StateMachineBehaviour
    {
        [SerializeField]
        private string startLoopName = "startDamageLoop";
        [SerializeField]
        private string stopLoopName = "stopDamageLoop";
        protected int startLoopID;
        protected int stopLoopID;

        protected DamageAnimationStateMachineBehaviour()
        {
        }

        private void Awake()
        {
            this.startLoopID = Animator.StringToHash(this.startLoopName);
            this.stopLoopID = Animator.StringToHash(this.stopLoopName);
        }
    }
}

