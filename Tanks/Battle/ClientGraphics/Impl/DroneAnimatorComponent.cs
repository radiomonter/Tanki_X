namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class DroneAnimatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private string idleStateName = "idle";
        [SerializeField]
        private string shootStateName = "shot";
        private int idleStateIndex;
        private int shootStateIndex;
        private Animator vulcanAnimator;

        private void Awake()
        {
            this.vulcanAnimator = base.GetComponent<Animator>();
            this.idleStateIndex = Animator.StringToHash(this.idleStateName);
            this.shootStateIndex = Animator.StringToHash(this.shootStateName);
        }

        public void StartIdle()
        {
            this.vulcanAnimator.SetTrigger(this.idleStateIndex);
        }

        public void StartShoot()
        {
            this.vulcanAnimator.SetTrigger(this.shootStateIndex);
        }
    }
}

