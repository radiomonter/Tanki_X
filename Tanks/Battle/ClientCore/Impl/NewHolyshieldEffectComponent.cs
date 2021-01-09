namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class NewHolyshieldEffectComponent : BehaviourComponent
    {
        private const float UP_OFFSET = 0.5f;
        private const float SIZE_TO_EFFECT_SCALE_RELATION = 0.5555556f;
        [SerializeField]
        private Animator hollyShieldEffect;
        private Animator animator;
        private Transform cameraTransform;
        private Transform root;
        private SphereCollider collider;
        private int showHash = Animator.StringToHash("show");
        private int hideHash = Animator.StringToHash("hide");
        private int invisHash = Animator.StringToHash("invisbility");
        private int alphaHash;
        private Material mat;
        private Vector3 previousCamPos;

        public GameObject InitEffect(Transform root, SkinnedMeshRenderer renderer, int colliderLayer)
        {
            this.root = root;
            this.alphaHash = Shader.PropertyToID("_Visibility");
            Vector3 size = renderer.localBounds.size;
            float[] values = new float[] { size.x, size.y, size.z };
            this.animator = Instantiate<Animator>(this.hollyShieldEffect, root.position, root.rotation, root);
            this.animator.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            Vector3 one = Vector3.one;
            float num2 = 0.5555556f * Mathf.Max(values);
            one.x = num2;
            one.y = num2;
            one.z = num2;
            this.animator.transform.localScale = one;
            this.animator.gameObject.SetActive(false);
            base.enabled = false;
            this.collider = this.animator.GetComponentInChildren<SphereCollider>();
            this.collider.gameObject.layer = colliderLayer;
            this.mat = this.animator.GetComponent<Renderer>().material;
            return this.animator.gameObject;
        }

        public void Play()
        {
            base.enabled = true;
            this.animator.gameObject.SetActive(true);
            this.animator.Play(this.showHash, 0);
        }

        public void Stop()
        {
            this.animator.Play(this.hideHash, 0);
        }

        private void Update()
        {
            if (this.animator.IsInTransition(0) && (this.animator.GetNextAnimatorStateInfo(0).shortNameHash == this.invisHash))
            {
                this.animator.gameObject.SetActive(false);
                base.enabled = false;
            }
        }

        public void UpdateAlpha(float alpha)
        {
            this.mat.SetFloat(this.alphaHash, alpha);
        }

        public Animator HollyShieldEffect =>
            this.hollyShieldEffect;

        public SphereCollider Collider
        {
            get => 
                this.collider;
            set => 
                this.collider = value;
        }
    }
}

