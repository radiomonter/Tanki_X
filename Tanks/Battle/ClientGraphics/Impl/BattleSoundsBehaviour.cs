namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class BattleSoundsBehaviour : MonoBehaviour
    {
        private const float DESTROY_DELAY = 1f;
        [SerializeField]
        private float minRemainigRoundTimeSec = 5f;
        [SerializeField]
        private int minDMScoreDiff = 5;
        [SerializeField]
        private int minTDMScoreDiff = 7;
        [SerializeField]
        private int minCTFScoreDiff = 2;
        [SerializeField]
        private AudioSource[] startSounds;
        [SerializeField]
        private AudioSource shortNeutralSound;
        [SerializeField]
        private AudioSource shortWinSound;
        [SerializeField]
        private AudioSource shortLostSound;
        [SerializeField]
        private AmbientSoundFilter victoryMelody;
        [SerializeField]
        private AmbientSoundFilter defeatMelody;
        [SerializeField]
        private AmbientSoundFilter neutralMelody;

        private void ApplyParentTransformData(Transform instanceTransform, Transform root)
        {
            instanceTransform.parent = root;
            instanceTransform.localPosition = Vector3.zero;
            instanceTransform.localRotation = Quaternion.identity;
            instanceTransform.localScale = Vector3.one;
        }

        private AmbientSoundFilter InstantiateAndPlay(AmbientSoundFilter source, Transform root, float delay)
        {
            AmbientSoundFilter filter = Instantiate<AmbientSoundFilter>(source);
            Transform instanceTransform = filter.transform;
            this.ApplyParentTransformData(instanceTransform, root);
            if (delay > 0f)
            {
                filter.Play(delay);
                return filter;
            }
            filter.Play(-1f);
            return filter;
        }

        private void InstantiateAndPlay(AudioSource source, Transform root, float delay)
        {
            AudioSource source2 = Instantiate<AudioSource>(source);
            Transform instanceTransform = source2.transform;
            this.ApplyParentTransformData(instanceTransform, root);
            if (delay > 0f)
            {
                source2.PlayScheduled(AudioSettings.dspTime + delay);
                DestroyObject(source2.gameObject, (delay + source2.clip.length) + 1f);
            }
            else
            {
                source2.Play();
                DestroyObject(source2.gameObject, source2.clip.length + 1f);
            }
        }

        public AmbientSoundFilter PlayNeutralMelody(Transform root, float delay = -1f) => 
            this.InstantiateAndPlay(this.neutralMelody, root, delay);

        public AmbientSoundFilter PlayNonNeutralMelody(Transform root, bool win, float delay = -1f) => 
            this.InstantiateAndPlay(!win ? this.defeatMelody : this.victoryMelody, root, delay);

        public void PlayShortNeutralSound(Transform root, float delay = -1f)
        {
            this.InstantiateAndPlay(this.shortNeutralSound, root, delay);
        }

        public void PlayShortNonNeutralSound(Transform root, bool win, float delay = -1f)
        {
            AudioSource source = !win ? this.shortLostSound : this.shortWinSound;
            this.InstantiateAndPlay(source, root, delay);
        }

        public void PlayStartSound(Transform root, float delay = -1f)
        {
            this.InstantiateAndPlay(this.startSounds[Random.Range(0, this.startSounds.Length)], root, delay);
        }

        public float MinRemainigRoundTimeSec =>
            this.minRemainigRoundTimeSec;

        public int MinDmScoreDiff =>
            this.minDMScoreDiff;

        public int MinTdmScoreDiff =>
            this.minTDMScoreDiff;

        public int MinCtfScoreDiff =>
            this.minCTFScoreDiff;
    }
}

