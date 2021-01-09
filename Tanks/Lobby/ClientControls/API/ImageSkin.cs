namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(UnityEngine.UI.Image))]
    public class ImageSkin : MonoBehaviour, SpriteRequest
    {
        [SerializeField]
        private string structureUid;
        private bool requestRegistered;
        [SerializeField]
        private string spriteUid;
        private UnityEngine.UI.Image image;
        public UnityEngine.UI.Image.Type type;
        private BaseElementScaleController requestHolder;

        public void Cancel()
        {
            if (this.Image != null)
            {
                this.Image.sprite = null;
                this.Image.overrideSprite = null;
            }
        }

        private void CancelRequest()
        {
            if (this.requestRegistered && (this.requestHolder != null))
            {
                this.requestHolder.UnregisterSpriteRequest(this);
                this.requestRegistered = false;
            }
        }

        protected void OnBeforeTransformParentChanged()
        {
            this.CancelRequest();
        }

        protected void OnDestroy()
        {
            this.CancelRequest();
            this.Cancel();
        }

        protected virtual void OnEnable()
        {
            this.RegisterRequest();
        }

        protected void OnTransformParentChanged()
        {
            this.RegisterRequest();
        }

        private void RegisterRequest()
        {
            if (string.IsNullOrEmpty(this.spriteUid))
            {
                if (Application.isPlaying)
                {
                    this.Image.sprite = null;
                    this.Image.enabled = false;
                }
            }
            else
            {
                if (Application.isPlaying && (this.Image.overrideSprite == null))
                {
                    this.Image.enabled = false;
                }
                BaseElementScaleControllerProvider componentInParent = base.GetComponentInParent<BaseElementScaleControllerProvider>();
                if ((componentInParent != null) && (componentInParent.BaseElementScaleController != null))
                {
                    this.requestHolder = componentInParent.BaseElementScaleController;
                    componentInParent.BaseElementScaleController.RegisterSpriteRequest(this);
                    this.requestRegistered = true;
                }
            }
        }

        protected void Reset()
        {
            this.ResetSkin();
        }

        public void ResetSkin()
        {
            this.Image.sprite = null;
        }

        public void Resolve(Sprite sprite)
        {
            if (this.Image == null)
            {
                this.CancelRequest();
            }
            else if (!Application.isPlaying)
            {
                this.Image.overrideSprite = sprite;
            }
            else
            {
                this.Image.sprite = sprite;
                this.Image.enabled = true;
            }
        }

        private UnityEngine.UI.Image Image
        {
            get
            {
                if ((this.image == null) && ((this != null) && (base.gameObject != null)))
                {
                    this.image = base.GetComponent<UnityEngine.UI.Image>();
                }
                return this.image;
            }
        }

        public string SpriteUid
        {
            get => 
                this.spriteUid;
            set
            {
                if (this.spriteUid != value)
                {
                    this.CancelRequest();
                    this.spriteUid = value;
                    this.RegisterRequest();
                }
            }
        }

        public string Uid =>
            this.SpriteUid;
    }
}

