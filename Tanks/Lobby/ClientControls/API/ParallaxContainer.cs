namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class ParallaxContainer : MonoBehaviour
    {
        [SerializeField]
        private bool isActive = true;
        [SerializeField]
        private Camera camera;
        [SerializeField]
        private RectTransform layer;
        [SerializeField]
        private RectTransform background;
        [SerializeField]
        private RectTransform container;
        [SerializeField]
        private float mainTransformRotateFactor = 10f;
        [SerializeField]
        private float layerMoveFactor = 10f;
        [SerializeField]
        private float backgroundMoveFactor = 20f;
        [SerializeField]
        private float lerpSpeed = 20f;
        private Vector3 mousePos;

        private void OnEnable()
        {
            this.camera = base.GetComponentInParent<Canvas>().worldCamera;
            this.mousePos = this.camera.WorldToScreenPoint(base.transform.position);
        }

        [ContextMenu("Reset layers")]
        public void ResetLayers()
        {
        }

        private void Update()
        {
            if (!this.isActive)
            {
                if ((this.container && (base.transform.eulerAngles.x != 0f)) || (base.transform.eulerAngles.y != 0f))
                {
                    base.transform.eulerAngles = new Vector3(Mathf.MoveTowardsAngle(base.transform.eulerAngles.x, 0f, (Time.deltaTime * this.mainTransformRotateFactor) * 10f), Mathf.MoveTowardsAngle(base.transform.eulerAngles.y, 0f, (Time.deltaTime * this.mainTransformRotateFactor) * 10f));
                }
            }
            else
            {
                float num = !this.container ? ((float) Screen.height) : this.container.rect.height;
                float num2 = !this.container ? ((float) Screen.width) : this.container.rect.width;
                Vector3 vector5 = this.camera.WorldToScreenPoint(base.transform.position);
                this.mousePos = (Vector3) Vector2.Lerp(this.mousePos, !this.isActive ? vector5 : Input.mousePosition, Time.deltaTime * this.lerpSpeed);
                float a = Mathf.Clamp((float) ((this.mousePos.x - vector5.x) / num2), (float) -1f, (float) 1f);
                float num4 = Mathf.Clamp((float) ((this.mousePos.y - vector5.y) / num), (float) -1f, (float) 1f);
                a = (a <= 0f) ? Mathf.Max(a, -1f) : Mathf.Min(a, 1f);
                num4 = (num4 <= 0f) ? Mathf.Max(num4, -1f) : Mathf.Min(num4, 1f);
                base.transform.eulerAngles = new Vector3(num4 * this.mainTransformRotateFactor, -a * this.mainTransformRotateFactor);
                if (this.layer)
                {
                    this.layer.anchoredPosition = new Vector2((-a * this.layerMoveFactor) + 2.6f, -num4 * this.layerMoveFactor);
                }
                if (this.background)
                {
                    this.background.anchoredPosition = new Vector2(-a * this.backgroundMoveFactor, -num4 * this.backgroundMoveFactor);
                }
            }
        }

        public bool IsActive
        {
            get => 
                this.isActive;
            set => 
                this.isActive = value;
        }
    }
}

