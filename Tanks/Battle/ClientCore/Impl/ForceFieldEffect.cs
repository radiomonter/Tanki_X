namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    public class ForceFieldEffect : MonoBehaviour
    {
        public float alpha = 1f;
        public MeshCollider outerMeshCollider;
        public MeshCollider innerMeshCollider;
        public MeshRenderer meshRenderer;
        public DomeWaveGenerator waveGenerator;
        public Material enemyMaterial;
        [SerializeField]
        private AudioSource hitSourceAsset;
        [SerializeField]
        private AudioClip[] hitClips;
        private Camera _cachedCamera;
        private Animator _animator;

        private void Awake()
        {
            this._animator = base.GetComponent<Animator>();
            this.waveGenerator.Init();
        }

        public void DrawWave(Vector3 hitPoint, bool playSound)
        {
            this.waveGenerator.GenerateWave(hitPoint);
            if (playSound)
            {
                this.InstantiateSound(hitPoint);
            }
        }

        public void Hide()
        {
            this._animator.SetTrigger("hide");
            this.outerMeshCollider.enabled = false;
            this.innerMeshCollider.enabled = false;
        }

        private void InstantiateSound(Vector3 point)
        {
            AudioSource source = Instantiate<AudioSource>(this.hitSourceAsset);
            int index = Random.Range(0, this.hitClips.Length);
            AudioClip clip = this.hitClips[index];
            source.clip = clip;
            source.transform.position = point;
            source.transform.rotation = Quaternion.identity;
            source.Play();
            DestroyObject(source.gameObject, clip.length + 0.3f);
        }

        public void LateUpdate()
        {
            this.meshRenderer.material.SetFloat("_MainAlpha", this.alpha);
        }

        private void OnEffectHidden()
        {
            DestroyObject(base.gameObject);
        }

        public void SetLayer(int layer)
        {
            base.gameObject.layer = layer;
            this.outerMeshCollider.gameObject.layer = layer;
            this.innerMeshCollider.gameObject.layer = layer;
        }

        public void Show()
        {
            this.meshRenderer.material.SetFloat("_MainAlpha", 0f);
            if (this.CachedCamera != null)
            {
                this.meshRenderer.material.shader.maximumLOD = (this.CachedCamera.depthTextureMode != DepthTextureMode.None) ? 300 : 150;
            }
            this._animator.SetTrigger("show");
            this.outerMeshCollider.enabled = true;
            this.innerMeshCollider.enabled = true;
        }

        public void SwitchToEnemyView()
        {
            this.meshRenderer.material = this.enemyMaterial;
            this.waveGenerator.Init();
        }

        public Camera CachedCamera
        {
            get
            {
                if (!this._cachedCamera)
                {
                    this._cachedCamera = Camera.main;
                }
                return this._cachedCamera;
            }
        }
    }
}

