namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public abstract class LimitedInstancingSoundEffectBehaviour : MonoBehaviour
    {
        private static double[] LAST_PLAY_TIMES = new double[] { -1.0, -1.0 };
        [SerializeField]
        private AudioSource source;
        [SerializeField]
        private float playDelay = -1f;

        protected LimitedInstancingSoundEffectBehaviour()
        {
        }

        private static bool CanInstantiateSoundEffect(int index, float minTimeForPlayingSec) => 
            (LAST_PLAY_TIMES[index] >= 0.0) ? ((AudioSettings.dspTime - LAST_PLAY_TIMES[index]) >= minTimeForPlayingSec) : true;

        protected static bool CreateSoundEffectInstance(LimitedInstancingSoundEffectBehaviour effectPrefab, int index, float minTimeForPlayingSec)
        {
            if (!CanInstantiateSoundEffect(index, minTimeForPlayingSec))
            {
                return false;
            }
            InstantiateAndPlaySoundEffectInstance(effectPrefab, index);
            return true;
        }

        private static void InstantiateAndPlaySoundEffectInstance(LimitedInstancingSoundEffectBehaviour effectPrefab, int index)
        {
            LimitedInstancingSoundEffectBehaviour behaviour = Instantiate<LimitedInstancingSoundEffectBehaviour>(effectPrefab);
            DontDestroyOnLoad(behaviour.gameObject);
            behaviour.Play(index);
        }

        private void Play(int index)
        {
            if (this.playDelay <= 0f)
            {
                this.source.Play();
                LAST_PLAY_TIMES[index] = AudioSettings.dspTime;
                DestroyObject(base.gameObject, this.source.clip.length);
            }
            else
            {
                double time = AudioSettings.dspTime + this.playDelay;
                this.source.PlayScheduled(time);
                LAST_PLAY_TIMES[index] = time;
                base.enabled = true;
            }
        }

        private void Update()
        {
            if (!this.source.isPlaying)
            {
                DestroyObject(base.gameObject);
            }
        }
    }
}

