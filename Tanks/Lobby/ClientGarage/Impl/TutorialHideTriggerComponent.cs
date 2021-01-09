namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class TutorialHideTriggerComponent : BehaviourComponent
    {
        [SerializeField]
        private float hideDelay;
        protected bool triggered;
        protected Entity tutorialStep;

        public void Activate(Entity tutorialStep)
        {
            this.tutorialStep = tutorialStep;
            base.gameObject.SetActive(true);
            if (!base.gameObject.activeInHierarchy)
            {
                this.ForceHide();
            }
        }

        public void ForceHide()
        {
            if (!this.triggered)
            {
                base.ScheduleEvent<CompleteActiveTutorialEvent>(new EntityStub());
                this.hideDelay = 0f;
                this.Triggered();
            }
        }

        private void HideTutorial()
        {
            TutorialCanvas.Instance.Hide();
            base.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            this.ForceHide();
        }

        protected virtual void Triggered()
        {
            if (!this.triggered)
            {
                base.CancelInvoke();
                this.triggered = true;
                if (this.hideDelay == 0f)
                {
                    this.HideTutorial();
                }
                else
                {
                    base.Invoke("HideTutorial", this.hideDelay);
                }
            }
        }
    }
}

