namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class SelfTargetHitFeedbackHUDInstanceComponent : BehaviourComponent
    {
        private const float HEIGHT_ROOT = 5f;
        [SerializeField]
        private EntityBehaviour entityBehaviour;
        [SerializeField]
        private Vector2 minSize = new Vector2(150f, 200f);
        [SerializeField]
        private Vector2 maxSize = new Vector2(300f, 400f);
        [SerializeField]
        private Vector2 relativeSizeCoeff = new Vector2(0.1f, 0.2f);
        [SerializeField]
        private float lengthPercent = 0.001f;
        [SerializeField, Range(0f, 1f), HideInInspector]
        private float lengthInterpolator;
        [SerializeField]
        private RectTransform rootRectTransform;
        [SerializeField]
        private RectTransform imageRectTransform;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Image image;
        [SerializeField]
        private int fps = 30;
        [SerializeField]
        private int frameCount = 6;
        [SerializeField, HideInInspector]
        private float alpha;
        private int appearID;
        private int disappearID;
        private int colorID;
        private Entity entity;
        private Material material;
        private float requiredLength;
        private float length;
        private float slopeAddition;
        private float width;
        private float animationFrameTime;
        private float animationTimer;
        private float frameOffset;
        private int textureID;
        private int currentFrameIndex;
        private SelfTargetHitEffectHUDData initialData;

        private float GetAxisAngle(Vector2 vec, Vector2 axis)
        {
            float num = Vector2.Angle(vec, axis);
            if (num > 90f)
            {
                num = 180f - num;
            }
            return num;
        }

        public void Init(Entity entity, SelfTargetHitEffectHUDData data)
        {
            this.InitTransform(data);
            this.InitMaterial();
            this.UpdateTransform(data);
            this.UpdateSpriteFrame();
            this.entity = entity;
            this.entityBehaviour.BuildEntity(entity);
            base.gameObject.SetActive(true);
        }

        private void InitMaterial()
        {
            this.animationTimer = 0f;
            this.animationFrameTime = 1f / ((float) this.fps);
            this.alpha = 0f;
            this.colorID = Shader.PropertyToID("_Color");
            this.material = Instantiate<Material>(this.image.material);
            this.image.material = this.material;
            this.textureID = Shader.PropertyToID("_MainTex");
            this.frameOffset = 1f / ((float) this.frameCount);
            this.material.SetTextureScale(this.textureID, new Vector2(this.frameOffset, 1f));
            this.currentFrameIndex = -1;
        }

        private void InitTransform(SelfTargetHitEffectHUDData data)
        {
            this.initialData = data;
            Vector2 cnvSize = data.CnvSize;
            float num = Mathf.Min(cnvSize.x, cnvSize.y);
            this.width = Mathf.Clamp(num * this.relativeSizeCoeff.x, this.minSize.x, this.maxSize.x);
            this.lengthInterpolator = 1f;
            this.requiredLength = Mathf.Clamp(num * this.relativeSizeCoeff.y, this.minSize.y, Mathf.Min(this.maxSize.y, Mathf.Max(data.Length * this.lengthPercent, this.minSize.y)));
            this.length = 0f;
        }

        private void LateUpdate()
        {
            this.UpdateSize();
            Color color = new Color(1f, 1f, 1f, this.alpha);
            this.material.SetColor(this.colorID, color);
        }

        private void OnDisappeared()
        {
            ECSBehaviour.EngineService.Engine.DeleteEntity(this.entity);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (this.animationTimer > 0f)
            {
                this.animationTimer -= deltaTime;
            }
            else
            {
                this.UpdateSpriteFrame();
                this.animationTimer = this.animationFrameTime;
            }
        }

        private void UpdateSize()
        {
            this.length = Mathf.Lerp(0f, this.requiredLength, this.lengthInterpolator);
            Vector2 vector = new Vector2(this.width, 5f);
            this.rootRectTransform.sizeDelta = vector;
            float num = this.length + this.slopeAddition;
            Vector2 vector2 = vector;
            vector2.y = num;
            float y = (num * 0.5f) - this.slopeAddition;
            this.imageRectTransform.sizeDelta = vector2;
            this.imageRectTransform.localPosition = new Vector3(0f, y, 0f);
        }

        private void UpdateSlope(SelfTargetHitEffectHUDData data)
        {
            float num = this.width * 0.5f;
            float axisAngle = this.GetAxisAngle(data.UpwardsNrm, Vector2.right);
            float num3 = this.GetAxisAngle(data.UpwardsNrm, Vector2.up);
            float f = num / Mathf.Tan(0.01745329f * ((Mathf.Abs((float) (Mathf.Abs((float) (data.BoundsPosition.x - 0.5f)) - 0.5f)) > 0.001f) ? axisAngle : num3));
            if (!float.IsInfinity(f) && !float.IsNaN(f))
            {
                this.slopeAddition = f;
            }
        }

        private void UpdateSpriteFrame()
        {
            int num = Random.Range(0, this.frameCount);
            if (num != this.currentFrameIndex)
            {
                this.currentFrameIndex = num;
            }
            else if ((num + 1) >= this.frameCount)
            {
                this.currentFrameIndex = 0;
            }
            this.material.SetTextureOffset(this.textureID, new Vector2(this.currentFrameIndex * this.frameOffset, 0f));
        }

        public void UpdateTransform(SelfTargetHitEffectHUDData data)
        {
            this.rootRectTransform.localPosition = (Vector3) data.BoundsPosCanvas;
            this.rootRectTransform.localRotation = Quaternion.LookRotation(Vector3.forward, data.UpwardsNrm);
            this.UpdateSlope(data);
            this.UpdateSize();
        }

        public SelfTargetHitEffectHUDData InitialData =>
            this.initialData;
    }
}

