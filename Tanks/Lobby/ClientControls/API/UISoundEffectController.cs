namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public abstract class UISoundEffectController : MonoBehaviour
    {
        private const string UI_SOUND_ROOT_NAME = "UISoundRoot";
        private const string SOUND_NAME_POSTFIX = "Sound";
        private static Transform uiTransformRoot;
        [SerializeField]
        private AudioClip clip;
        [SerializeField]
        private AudioSource sourceAsset;
        private AudioSource sourceInstance;
        protected bool alive;

        protected UISoundEffectController()
        {
        }

        private void Awake()
        {
            this.alive = true;
            this.PrepareAudioSourceInstance();
        }

        private void OnApplicationQuit()
        {
            this.alive = false;
        }

        private void OnDestroy()
        {
            if (this.alive && this.sourceInstance)
            {
                Destroy(this.sourceInstance.gameObject);
            }
        }

        public void PlaySoundEffect()
        {
            this.PrepareAudioSourceInstance();
            this.sourceInstance.Stop();
            this.sourceInstance.Play();
        }

        private void PrepareAudioSourceInstance()
        {
            if (this.sourceInstance == null)
            {
                this.sourceInstance = Instantiate<AudioSource>(this.sourceAsset);
                GameObject gameObject = this.sourceInstance.gameObject;
                Transform transform = this.sourceInstance.transform;
                transform.parent = UITransformRoot;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                gameObject.name = $"{base.gameObject.name}_{this.HandlerName}_{"Sound"}";
                this.sourceInstance.clip = this.clip;
            }
        }

        public static Transform UITransformRoot
        {
            get
            {
                if (uiTransformRoot == null)
                {
                    uiTransformRoot = new GameObject("UISoundRoot").transform;
                    DontDestroyOnLoad(uiTransformRoot.gameObject);
                    uiTransformRoot.position = Vector3.zero;
                    uiTransformRoot.rotation = Quaternion.identity;
                }
                return uiTransformRoot;
            }
        }

        public abstract string HandlerName { get; }
    }
}

