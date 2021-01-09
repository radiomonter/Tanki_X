namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class MapAnimatorTriggerComponent : MonoBehaviour
    {
        public Transform animator;
        private Animator animatorController;
        public string triggerEnable;
        public string triggerDisable;
        private int count;

        private void OnTriggerEnter()
        {
            if (this.count == 0)
            {
                this.animatorController.SetBool(this.triggerEnable, true);
            }
            this.count++;
        }

        private void OnTriggerExit()
        {
            if (this.count == 1)
            {
                this.animatorController.SetBool(this.triggerEnable, false);
            }
            this.count--;
        }

        private void Start()
        {
            this.animatorController = this.animator.GetComponent<Animator>();
        }
    }
}

