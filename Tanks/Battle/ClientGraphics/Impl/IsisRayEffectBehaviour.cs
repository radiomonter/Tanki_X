namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class IsisRayEffectBehaviour : MonoBehaviour
    {
        private const float TAU = 6.283185f;
        [Header("Assets"), SerializeField]
        private ParticleSystem expandingBlob;
        [SerializeField]
        private ParticleSystem contractingBlob;
        [SerializeField]
        private LineRenderer[] rays;
        [SerializeField]
        private Material damagingBallMaterial;
        [SerializeField]
        private Material damagingRayMaterial;
        [SerializeField]
        private Material healingBallMaterial;
        [SerializeField]
        private Material healingRayMaterial;
        [Space(1f), Header("Initialization parameters"), SerializeField]
        private int curvesCount = 5;
        [SerializeField]
        private float minCurveMagnitude = 0.05f;
        [SerializeField]
        private float maxCurveMagnitude = 0.1f;
        [Space(1f), Header("Dynamic parameters"), SerializeField]
        private float offsetToCamera = 0.5f;
        [SerializeField]
        private float smoothingSpeed = 1f;
        [SerializeField]
        private float[] textureTilings;
        [SerializeField]
        private float[] textureOffsets;
        [SerializeField]
        private float verticesSpacing = 0.5f;
        [SerializeField]
        private float curveLength = 5f;
        [SerializeField]
        private float curveSpeed = 0.5f;
        [SerializeField]
        private Color healColor;
        [SerializeField]
        private Color damageColor;
        private float textureScrollDirection = 1f;
        private Vector3[] bezierPoints = new Vector3[3];
        private AnimationCurve curve;
        private ParticleSystemRenderer expandingBlobRenderer;
        private ParticleSystemRenderer contractingBlobRenderer;
        private Light expandingLight;
        private Light contractingLight;
        private Animation expandingAnimation;
        private Animation contractingAnimation;
        private ParticleSystem nearBlob;
        private ParticleSystem farBlob;
        private Light nearLight;
        private Light farLight;
        private Animation nearAnimation;
        private Animation farAnimation;
        private ParticleSystemRenderer nearBlobRenderer;
        private ParticleSystemRenderer farBlobRenderer;
        [SerializeField]
        private Material[] damagingRayMaterials;
        [SerializeField]
        private Material[] healingRayMaterials;
        private Vector3 endLocalPosition;
        private Camera _cachedCamera;

        private static void DisableBlob(ParticleSystem blob, Light light, Animation animation)
        {
            blob.Stop();
            blob.Clear();
            blob.enableEmission = false;
            light.enabled = false;
            animation.enabled = false;
        }

        public void DisableTarget()
        {
            foreach (LineRenderer renderer in this.rays)
            {
                if (renderer)
                {
                    renderer.enabled = false;
                }
            }
            this.SetHealingMode();
            DisableBlob(this.farBlob, this.farLight, this.farAnimation);
            base.enabled = false;
        }

        private static void EnableBlob(ParticleSystem blob, Light light, Animation animation)
        {
            blob.enableEmission = true;
            blob.Emit(1);
            blob.Play();
            light.enabled = true;
            animation.enabled = true;
        }

        public void EnableTargetForDamaging()
        {
            base.enabled = true;
            EnableBlob(this.farBlob, this.farLight, this.farAnimation);
            this.SetDamagingMode();
            for (int i = 0; i < this.rays.Length; i++)
            {
                this.rays[i].enabled = true;
            }
        }

        public void EnableTargetForHealing()
        {
            base.enabled = true;
            EnableBlob(this.farBlob, this.farLight, this.farAnimation);
            this.SetHealingMode();
            for (int i = 0; i < this.rays.Length; i++)
            {
                this.rays[i].enabled = true;
            }
        }

        public void Hide()
        {
            for (int i = 0; i < this.rays.Length; i++)
            {
                if (this.rays[i])
                {
                    this.rays[i].enabled = false;
                }
            }
            this.SetHealingMode();
            DisableBlob(this.nearBlob, this.nearLight, this.nearAnimation);
            DisableBlob(this.farBlob, this.farLight, this.farAnimation);
            base.enabled = false;
        }

        public void Init()
        {
            this.damagingRayMaterials = new Material[this.rays.Length];
            this.healingRayMaterials = new Material[this.rays.Length];
            for (int i = 0; i < this.rays.Length; i++)
            {
                this.damagingRayMaterials[i] = Instantiate<Material>(this.damagingRayMaterial);
                this.healingRayMaterials[i] = Instantiate<Material>(this.healingRayMaterial);
            }
            this.expandingBlobRenderer = this.expandingBlob.GetComponent<ParticleSystemRenderer>();
            this.contractingBlobRenderer = this.contractingBlob.GetComponent<ParticleSystemRenderer>();
            this.expandingLight = this.expandingBlob.GetComponent<Light>();
            this.contractingLight = this.contractingBlob.GetComponent<Light>();
            this.expandingAnimation = this.expandingBlob.GetComponent<Animation>();
            this.contractingAnimation = this.contractingBlob.GetComponent<Animation>();
            this.InitCurve();
            this.Hide();
        }

        private void InitCurve()
        {
            float num = Random.value;
            Keyframe[] keys = new Keyframe[5 * this.curvesCount];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].time = ((float) i) / ((float) (keys.Length - 1));
                keys[i].value = Mathf.Sin(((keys[i].time * this.curvesCount) + num) * 6.283185f) * Random.Range(this.minCurveMagnitude, this.maxCurveMagnitude);
            }
            this.curve = new AnimationCurve(keys);
        }

        private Vector3 MovePoint(Vector3 moveThis, Vector3 from, Vector3 to, float speed, float randomness)
        {
            Vector3 vector = (Vector3) ((Quaternion.LookRotation(to - from) * Random.insideUnitCircle) * randomness);
            return Vector3.MoveTowards(moveThis, to + vector, Time.deltaTime * speed);
        }

        private void SetBlobs(ParticleSystem nearBlob, ParticleSystemRenderer nearBlobRenderer, Light nearLight, Animation nearAnimation, ParticleSystem farBlob, ParticleSystemRenderer farBlobRenderer, Light farLight, Animation farAnimation)
        {
            this.nearBlob = nearBlob;
            this.nearBlobRenderer = nearBlobRenderer;
            this.nearBlob.transform.localPosition = Vector3.zero;
            this.nearLight = nearLight;
            this.nearAnimation = nearAnimation;
            this.farBlob = farBlob;
            this.farBlobRenderer = farBlobRenderer;
            this.farBlob.transform.localPosition = Vector3.zero;
            this.farLight = farLight;
            this.farAnimation = farAnimation;
        }

        private void SetDamagingMode()
        {
            this.textureScrollDirection = 1f;
            this.SetBlobs(this.contractingBlob, this.contractingBlobRenderer, this.contractingLight, this.contractingAnimation, this.expandingBlob, this.expandingBlobRenderer, this.expandingLight, this.expandingAnimation);
            this.nearBlobRenderer.sharedMaterial = this.damagingBallMaterial;
            this.farBlobRenderer.sharedMaterial = this.damagingBallMaterial;
            this.nearLight.color = this.damageColor;
            this.farLight.color = this.damageColor;
            for (int i = 0; i < this.rays.Length; i++)
            {
                this.rays[i].material = this.damagingRayMaterials[i];
            }
        }

        private void SetHealingMode()
        {
            this.textureScrollDirection = -1f;
            this.SetBlobs(this.expandingBlob, this.expandingBlobRenderer, this.expandingLight, this.expandingAnimation, this.contractingBlob, this.contractingBlobRenderer, this.contractingLight, this.contractingAnimation);
            this.nearBlobRenderer.sharedMaterial = this.healingBallMaterial;
            this.farBlobRenderer.sharedMaterial = this.healingBallMaterial;
            this.nearLight.color = this.healColor;
            this.farLight.color = this.healColor;
            for (int i = 0; i < this.rays.Length; i++)
            {
                this.rays[i].material = this.healingRayMaterials[i];
            }
        }

        public void Show()
        {
            EnableBlob(this.nearBlob, this.nearLight, this.nearAnimation);
        }

        private void Update()
        {
            for (int i = 0; i < this.rays.Length; i++)
            {
                Vector2 mainTextureOffset = this.rays[i].sharedMaterial.mainTextureOffset;
                mainTextureOffset.x = (mainTextureOffset.x + ((this.textureOffsets[i] * this.textureScrollDirection) * Time.deltaTime)) % 1f;
                this.rays[i].sharedMaterial.mainTextureOffset = mainTextureOffset;
            }
        }

        public void UpdateTargetPosition(Transform targetTransform, Vector3 targetLocalPosition, float[] speedMultipliers, float[] pointsRandomness)
        {
            this.bezierPoints[0] = base.transform.position;
            float speed = speedMultipliers[2] * this.smoothingSpeed;
            Vector3 from = targetTransform.InverseTransformPoint(base.transform.position);
            this.endLocalPosition = this.MovePoint(this.endLocalPosition, from, targetLocalPosition, speed, pointsRandomness[2]);
            this.bezierPoints[2] = targetTransform.TransformPoint(this.endLocalPosition);
            Vector3 to = Vector3.Lerp(base.transform.position, this.bezierPoints[2], 0.5f);
            this.bezierPoints[1] = this.MovePoint(this.bezierPoints[1], base.transform.position, to, speedMultipliers[1] * this.smoothingSpeed, pointsRandomness[1]);
            this.nearBlob.transform.position = this.bezierPoints[0];
            this.farBlob.transform.position = this.bezierPoints[2] + ((this.CachedCamera.transform.position - this.bezierPoints[2]).normalized * this.offsetToCamera);
            Vector3 lhs = this.bezierPoints[2] - this.bezierPoints[0];
            float magnitude = lhs.magnitude;
            Vector3 normalized = Vector3.Cross(lhs, this.CachedCamera.transform.forward).normalized;
            float num4 = magnitude / (this.curveLength * this.curvesCount);
            int count = 1 + Mathf.CeilToInt(magnitude / this.verticesSpacing);
            int index = 0;
            while (index < this.rays.Length)
            {
                this.rays[index].SetVertexCount(count);
                int num7 = 0;
                while (true)
                {
                    if (num7 >= count)
                    {
                        Vector2 mainTextureScale = this.rays[index].sharedMaterial.mainTextureScale;
                        mainTextureScale.x = magnitude / this.textureTilings[index];
                        this.rays[index].sharedMaterial.mainTextureScale = mainTextureScale;
                        index++;
                        break;
                    }
                    float t = ((float) num7) / ((float) (count - 1));
                    Vector3 vector7 = Bezier.PointOnCurve(t, this.bezierPoints[0], this.bezierPoints[1], this.bezierPoints[2]);
                    float time = ((t * num4) + ((this.textureScrollDirection * this.curveSpeed) * Time.time)) % 1f;
                    Vector3 vector8 = normalized * this.curve.Evaluate(time);
                    this.rays[index].SetPosition(num7, vector7 + vector8);
                    num7++;
                }
            }
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

