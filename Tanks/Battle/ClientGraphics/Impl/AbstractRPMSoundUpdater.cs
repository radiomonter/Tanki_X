namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public abstract class AbstractRPMSoundUpdater : MonoBehaviour
    {
        [SerializeField]
        protected bool alive;
        [SerializeField]
        protected HullSoundEngineController engine;
        [SerializeField]
        protected AbstractRPMSoundModifier parentModifier;
        [SerializeField]
        protected RPMSoundBehaviour rpmSoundBehaviour;

        protected AbstractRPMSoundUpdater()
        {
        }

        protected virtual void Awake()
        {
            this.Stop();
            this.alive = true;
        }

        public virtual void Build(HullSoundEngineController engine, AbstractRPMSoundModifier abstractRPMSoundModifier, RPMSoundBehaviour rpmSoundBehaviour)
        {
            RPMVolumeUpdaterFinishBehaviour component = base.gameObject.GetComponent<RPMVolumeUpdaterFinishBehaviour>();
            if (component != null)
            {
                DestroyImmediate(component);
            }
            this.engine = engine;
            this.parentModifier = abstractRPMSoundModifier;
            this.rpmSoundBehaviour = rpmSoundBehaviour;
        }

        private void OnApplicationQuit()
        {
            this.alive = false;
        }

        protected virtual void OnDisable()
        {
            if (this.alive)
            {
                this.parentModifier.Source.Pause();
            }
        }

        protected virtual void OnEnable()
        {
            this.parentModifier.Source.Play();
        }

        public virtual void Play()
        {
            base.enabled = true;
        }

        public virtual void Stop()
        {
            base.enabled = false;
        }
    }
}

