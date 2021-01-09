namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AmbientZoneSoundEffect : MonoBehaviour
    {
        private Transform listener;
        [SerializeField]
        private AmbientInnerSoundZone[] innerZones;
        [SerializeField]
        private AmbientOuterSoundZone outerZone;
        private int innerZonesLength;
        private AmbientSoundZone currentZone;
        private bool needToDestroy;
        private HashSet<AmbientSoundZone> playingZones;

        private AmbientSoundZone DefineCurrentAmbientZone()
        {
            for (int i = 0; i < this.innerZonesLength; i++)
            {
                AmbientInnerSoundZone zone = this.innerZones[i];
                if (zone.IsActualZone(this.listener))
                {
                    return zone;
                }
            }
            return this.outerZone;
        }

        public void DisableZoneTransition()
        {
            base.enabled = false;
        }

        public void FinalizeEffect()
        {
            if (this.needToDestroy && (this.playingZones.Count <= 0))
            {
                DestroyObject(base.gameObject);
            }
        }

        public void Play(Transform listener)
        {
            this.listener = listener;
            this.playingZones = new HashSet<AmbientSoundZone>();
            this.innerZonesLength = this.innerZones.Length;
            this.needToDestroy = false;
            for (int i = 0; i < this.innerZonesLength; i++)
            {
                this.innerZones[i].InitInnerZone();
            }
            base.transform.parent = listener;
            base.transform.localPosition = Vector3.zero;
            base.transform.localRotation = Quaternion.identity;
            this.currentZone = this.DefineCurrentAmbientZone();
            this.currentZone.FadeIn();
            base.enabled = true;
        }

        public void RegisterPlayingAmbientZone(AmbientSoundZone zone)
        {
            this.playingZones.Add(zone);
        }

        public void Stop()
        {
            this.needToDestroy = true;
            for (int i = 0; i < this.innerZonesLength; i++)
            {
                this.innerZones[i].FinalizeInnerZone();
            }
            this.currentZone.FadeOut();
            this.DisableZoneTransition();
        }

        public void UnregisterPlayingAmbientZone(AmbientSoundZone zone)
        {
            this.playingZones.Remove(zone);
        }

        private void Update()
        {
            AmbientSoundZone objA = this.DefineCurrentAmbientZone();
            if (!ReferenceEquals(objA, this.currentZone))
            {
                this.currentZone.FadeOut();
                objA.FadeIn();
                this.currentZone = objA;
            }
        }
    }
}

