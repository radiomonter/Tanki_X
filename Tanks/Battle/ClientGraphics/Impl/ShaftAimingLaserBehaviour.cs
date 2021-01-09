namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShaftAimingLaserBehaviour : MonoBehaviour
    {
        private const string SHADER_COLOR_NAME = "_TintColor";
        [SerializeField]
        private float fadeInTimeSec = 3f;
        [SerializeField]
        private float fadeOutTimeSec = 0.2f;
        [SerializeField]
        private float maxStartAlpha = 0.9f;
        [SerializeField]
        private float texScale = 5f;
        [SerializeField]
        private float laserWidth = 0.05f;
        [SerializeField]
        private float laserSourceOffset = 6f;
        [SerializeField]
        private float laserBeginLength = 1f;
        [SerializeField]
        private float speed1 = 0.05f;
        [SerializeField]
        private float speed2 = 0.075f;
        [SerializeField]
        private float delta = 3f;
        private Sprite3D spot;
        private List<LineRenderer> beginLines;
        private List<LineRenderer> baseLines;
        private LaserStates state;
        private float stateTime;
        private float initialAlpha;
        private float currentAlpha;
        private float segment;
        private float angle1;
        private float angle2;
        private Texture2D baseLaserTex;
        private Camera _cachedCamera;

        private void FadeIn()
        {
            if (this.stateTime >= this.fadeInTimeSec)
            {
                this.state = LaserStates.DEFAULT;
                this.UpdateAlphaForAllParts(1f);
            }
            else
            {
                this.stateTime += Time.deltaTime;
                this.UpdateAlphaForAllParts(Mathf.Lerp(this.initialAlpha, 1f, this.stateTime / this.fadeInTimeSec));
            }
        }

        private void FadeOut(bool killAfterFade)
        {
            if (this.stateTime < this.fadeOutTimeSec)
            {
                this.stateTime += Time.deltaTime;
                this.UpdateAlphaForAllParts(Mathf.Lerp(this.initialAlpha, 0f, this.stateTime / this.fadeOutTimeSec));
            }
            else if (killAfterFade)
            {
                Destroy(base.gameObject);
            }
            else
            {
                this.state = LaserStates.DEFAULT;
                this.UpdateAlphaForAllParts(0f);
                this.spot.enabled = false;
                this.beginLines[0].enabled = false;
                this.beginLines[1].enabled = false;
                this.baseLines[0].enabled = false;
                this.baseLines[1].enabled = false;
                base.enabled = false;
            }
        }

        public void Hide()
        {
            this.stateTime = 0f;
            this.initialAlpha = this.currentAlpha;
            this.state = LaserStates.FADE_OUT;
        }

        public void Init()
        {
            this.spot = base.GetComponentsInChildren<Sprite3D>(true)[0];
            this.beginLines = new List<LineRenderer>();
            this.baseLines = new List<LineRenderer>();
            this.InitializeLineRendererList<ShaftLaserBeginUnityComponent>(this.beginLines);
            this.InitializeLineRendererList<ShaftLaserBaseUnityComponent>(this.baseLines);
            this.baseLaserTex = (Texture2D) this.baseLines[0].material.mainTexture;
            this.segment = ((this.texScale * this.laserWidth) * this.baseLaserTex.height) / ((float) this.baseLaserTex.width);
            this.angle1 = 0f;
            this.angle2 = 0f;
            this.initialAlpha = 0f;
            this.currentAlpha = 0f;
            this.stateTime = 0f;
            this.UpdateAlphaForAllParts(this.initialAlpha);
            this.state = LaserStates.DEFAULT;
        }

        private void InitializeLineRendererList<T>(List<LineRenderer> lineRenderers) where T: Component
        {
            foreach (T local in base.GetComponentsInChildren<T>())
            {
                LineRenderer component = local.gameObject.GetComponent<LineRenderer>();
                component.startWidth = this.laserWidth;
                component.endWidth = this.laserWidth;
                lineRenderers.Add(local.gameObject.GetComponent<LineRenderer>());
            }
        }

        public void Kill()
        {
            base.enabled = true;
            if (this.state != LaserStates.FADE_OUT)
            {
                this.stateTime = 0f;
            }
            this.state = LaserStates.DEAD;
        }

        public void SetColor(Color color)
        {
            Color color2 = new Color(color.r, color.g, color.b, this.currentAlpha);
            this.spot.material.SetColor("_TintColor", color2);
            this.beginLines[0].material.SetColor("_TintColor", color2);
            this.beginLines[1].material.SetColor("_TintColor", color2);
            this.baseLines[0].material.SetColor("_TintColor", color2);
            this.baseLines[1].material.SetColor("_TintColor", color2);
        }

        public void Show()
        {
            this.stateTime = 0f;
            this.initialAlpha = this.currentAlpha;
            this.state = LaserStates.FADE_IN;
            this.spot.enabled = true;
            this.beginLines[0].enabled = true;
            this.beginLines[1].enabled = true;
            this.baseLines[0].enabled = true;
            this.baseLines[1].enabled = true;
            base.enabled = true;
        }

        private void Update()
        {
            this.angle1 += this.speed1 * Time.deltaTime;
            this.angle2 += this.speed2 * Time.deltaTime;
            Vector2 vector = new Vector2(-((Mathf.Sin(this.angle1) * this.delta) / this.segment), 0f);
            Vector2 vector2 = new Vector2(-((Mathf.Sin(this.angle2) * this.delta) / this.segment), 0f);
            this.baseLines[0].material.mainTextureOffset = vector;
            this.baseLines[1].material.mainTextureOffset = vector2;
            LaserStates state = this.state;
            if (state == LaserStates.FADE_IN)
            {
                this.FadeIn();
            }
            else if (state == LaserStates.FADE_OUT)
            {
                this.FadeOut(false);
            }
            else if (state == LaserStates.DEAD)
            {
                this.FadeOut(true);
            }
        }

        private void UpdateAlpha(float alpha, params Material[] materials)
        {
            foreach (Material material in materials)
            {
                ClientGraphicsUtil.UpdateAlpha(material, "_TintColor", alpha);
            }
        }

        private void UpdateAlphaForAllParts(float alpha)
        {
            this.currentAlpha = alpha;
            Material[] materials = new Material[] { this.spot.material, this.beginLines[0].material, this.beginLines[1].material, this.baseLines[0].material, this.baseLines[1].material };
            this.UpdateAlpha(alpha, materials);
        }

        public void UpdateTargetPosition(Vector3 startPosition, Vector3 targetPosition, bool showLaser, bool showSpot)
        {
            this.spot.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(startPosition - targetPosition));
            this.spot.gameObject.transform.position = targetPosition;
            Vector3 vector = Vector3.Normalize(this.CachedCamera.transform.position - targetPosition);
            Vector3 position = targetPosition + (vector * this.spot.offsetToCamera);
            Vector3 vector3 = position - startPosition;
            Vector3 vector4 = startPosition + (vector3.normalized * this.laserSourceOffset);
            Vector3 vector5 = vector4 + (vector3.normalized * this.laserBeginLength);
            this.beginLines[0].SetPosition(0, vector4);
            this.beginLines[0].SetPosition(1, vector5);
            this.beginLines[1].SetPosition(0, vector4);
            this.beginLines[1].SetPosition(1, vector5);
            this.baseLines[0].SetPosition(0, vector5);
            this.baseLines[0].SetPosition(1, position);
            this.baseLines[1].SetPosition(0, vector5);
            this.baseLines[1].SetPosition(1, position);
            this.spot.enabled = showSpot;
            this.beginLines[0].enabled = showLaser;
            this.beginLines[1].enabled = showLaser;
            this.baseLines[0].enabled = showLaser;
            this.baseLines[1].enabled = showLaser;
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

        private enum LaserStates
        {
            FADE_IN,
            FADE_OUT,
            DEAD,
            DEFAULT
        }
    }
}

