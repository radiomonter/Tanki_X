namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ContainerComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener
    {
        private Entity entity;
        [SerializeField]
        private Animator containerAnimator;
        [SerializeField]
        private ParticleSystem[] dustParticles;
        [SerializeField]
        private AudioSource openSound;
        [SerializeField]
        private AudioSource closeSound;
        [SerializeField]
        private string idleClosedAnimationName;
        [SerializeField]
        private string closingAnimationName;

        public void CloseContainer()
        {
            if (!this.InClosingState())
            {
                this.openSound.Stop();
                this.closeSound.Play();
                this.containerAnimator.ResetTrigger("close");
                this.containerAnimator.SetTrigger("close");
            }
        }

        public void ContainerOpend()
        {
            base.ScheduleEvent(new OpenContainerAnimationShownEvent(), this.entity);
        }

        private bool InClosingState() => 
            this.containerAnimator.GetCurrentAnimatorStateInfo(0).IsName(this.idleClosedAnimationName) || this.containerAnimator.GetCurrentAnimatorStateInfo(0).IsName(this.closingAnimationName);

        void AttachToEntityListener.AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        public void PlayDustAnimators()
        {
            foreach (ParticleSystem system in this.dustParticles)
            {
                system.Play();
            }
        }

        public void ShowOpenContainerAnimation()
        {
            this.PlayDustAnimators();
            this.openSound.Play();
            this.closeSound.Stop();
            this.containerAnimator.ResetTrigger("open");
            this.containerAnimator.SetTrigger("open");
        }

        public string assetGuid { get; set; }
    }
}

