namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class WeaponFeedbackSoundBehaviour : MonoBehaviour
    {
        private static readonly HashSet<AudioSource> WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL = new HashSet<AudioSource>();
        [SerializeField]
        private AudioSource source;
        private bool alive;

        public static void ClearHitFeedbackSounds()
        {
            HashSet<AudioSource>.Enumerator enumerator = WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!enumerator.Current)
                {
                    continue;
                }
                enumerator.Current.Stop();
            }
            WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.Clear();
        }

        private void OnApplicationQuit()
        {
            this.alive = false;
        }

        private void OnDestroy()
        {
            if (this.alive)
            {
                WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.Remove(this.source);
            }
        }

        public void Play(float delay, float volume, bool removeOnKillSound)
        {
            this.source.volume = volume;
            if (delay <= 0f)
            {
                this.source.Play();
                DestroyObject(this.source.gameObject, this.source.clip.length);
            }
            else
            {
                this.source.PlayDelayed(delay);
                DestroyObject(this.source.gameObject, this.source.clip.length + delay);
            }
            if (removeOnKillSound)
            {
                WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.Add(this.source);
            }
            this.alive = true;
            base.enabled = true;
        }

        public void Stop()
        {
            this.source.Stop();
        }
    }
}

