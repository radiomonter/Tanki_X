namespace CurvedUI
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CurvedUI/CurvedUISettings"), RequireComponent(typeof(Canvas))]
    public class CurvedUISettings : MonoBehaviour
    {
        [SerializeField]
        private CurvedUIShape shape;
        [SerializeField]
        private float quality = 1f;
        [SerializeField]
        private bool interactable = true;
        [SerializeField]
        private bool blocksRaycasts = true;
        [SerializeField]
        private bool raycastMyLayerOnly = true;
        [SerializeField]
        private bool forceUseBoxCollider = true;
        [SerializeField]
        private int angle = 90;
        [SerializeField]
        private bool preserveAspect = true;
        [SerializeField]
        private int vertAngle = 90;
        [SerializeField]
        private float ringFill = 0.5f;
        [SerializeField]
        private int ringExternalDiamater = 0x3e8;
        [SerializeField]
        private bool ringFlipVertical;
        private int baseCircleSegments = 0x18;
        private Vector2 savedRectSize;
        private float savedRadius;
        private Canvas myCanvas;
        private UnityEngine.RectTransform m_rectTransform;

        public void AddEffectToChildren()
        {
            foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>(true))
            {
                if (graphic.GetComponent<CurvedUIVertexEffect>() == null)
                {
                    graphic.gameObject.AddComponent<CurvedUIVertexEffect>();
                    graphic.SetAllDirty();
                }
            }
            foreach (InputField field in base.GetComponentsInChildren<InputField>(true))
            {
                if (field.GetComponent<CurvedUIInputFieldCaret>() == null)
                {
                    field.gameObject.AddComponent<CurvedUIInputFieldCaret>();
                }
            }
            foreach (TextMeshProUGUI ougui in base.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (ougui.GetComponent<CurvedUITMP>() == null)
                {
                    ougui.gameObject.AddComponent<CurvedUITMP>();
                    ougui.SetAllDirty();
                }
            }
        }

        private void Awake()
        {
            if (this.RaycastMyLayerOnly)
            {
                base.gameObject.layer ??= 5;
            }
            this.savedRectSize = this.RectTransform.rect.size;
        }

        public Vector3 CanvasToCurvedCanvas(Vector3 pos)
        {
            pos = this.VertexPositionToCurvedCanvas(pos);
            return ((float.IsNaN(pos.x) || float.IsInfinity(pos.x)) ? Vector3.zero : base.transform.localToWorldMatrix.MultiplyPoint3x4(pos));
        }

        public Vector3 CanvasToCurvedCanvasNormal(Vector3 pos)
        {
            pos = this.VertexPositionToCurvedCanvas(pos);
            switch (this.Shape)
            {
                case CurvedUIShape.CYLINDER:
                    return base.transform.localToWorldMatrix.MultiplyVector((pos - new Vector3(0f, 0f, -this.GetCyllinderRadiusInCanvasSpace())).ModifyY(0f)).normalized;

                case CurvedUIShape.RING:
                    return -base.transform.forward;

                case CurvedUIShape.SPHERE:
                {
                    Vector3 vector3 = !this.PreserveAspect ? Vector3.zero : new Vector3(0f, 0f, -this.GetCyllinderRadiusInCanvasSpace());
                    return base.transform.localToWorldMatrix.MultiplyVector(pos - vector3).normalized;
                }
                case CurvedUIShape.CYLINDER_VERTICAL:
                    return base.transform.localToWorldMatrix.MultiplyVector((pos - new Vector3(0f, 0f, -this.GetCyllinderRadiusInCanvasSpace())).ModifyX(0f)).normalized;
            }
            return Vector3.zero;
        }

        private unsafe Vector3 CanvasToCyllinder(Vector3 pos)
        {
            float f = ((pos.x / this.savedRectSize.x) * this.Angle) * 0.01745329f;
            pos.x = Mathf.Sin(f) * (this.SavedRadius + pos.z);
            Vector3* vectorPtr1 = &pos;
            vectorPtr1->z += (Mathf.Cos(f) * (this.SavedRadius + pos.z)) - (this.SavedRadius + pos.z);
            return pos;
        }

        private unsafe Vector3 CanvasToCyllinderVertical(Vector3 pos)
        {
            float f = ((pos.y / this.savedRectSize.y) * this.Angle) * 0.01745329f;
            pos.y = Mathf.Sin(f) * (this.SavedRadius + pos.z);
            Vector3* vectorPtr1 = &pos;
            vectorPtr1->z += (Mathf.Cos(f) * (this.SavedRadius + pos.z)) - (this.SavedRadius + pos.z);
            return pos;
        }

        private Vector3 CanvasToRing(Vector3 pos)
        {
            float num = pos.y.Remap((this.savedRectSize.y * 0.5f) * (!this.RingFlipVertical ? ((float) (-1)) : ((float) 1)), (-this.savedRectSize.y * 0.5f) * (!this.RingFlipVertical ? ((float) (-1)) : ((float) 1)), (this.RingExternalDiameter * (1f - this.RingFill)) * 0.5f, this.RingExternalDiameter * 0.5f);
            float f = ((float) (pos.x / this.savedRectSize.x)).Remap((float) -0.5f, (float) 0.5f, (float) 1.570796f, (float) ((this.angle * 0.01745329f) + 1.570796f));
            pos.x = num * Mathf.Cos(f);
            pos.y = num * Mathf.Sin(f);
            return pos;
        }

        private unsafe Vector3 CanvasToSphere(Vector3 pos)
        {
            float savedRadius = this.SavedRadius;
            float verticalAngle = this.VerticalAngle;
            if (this.PreserveAspect)
            {
                verticalAngle = this.angle * (this.savedRectSize.y / this.savedRectSize.x);
                savedRadius += (this.Angle <= 0) ? pos.z : -pos.z;
            }
            else
            {
                savedRadius = (this.savedRectSize.x / 2f) + pos.z;
                if (verticalAngle == 0f)
                {
                    return Vector3.zero;
                }
            }
            float f = ((float) (pos.x / this.savedRectSize.x)).Remap(((float) -0.5f), ((float) 0.5f), ((float) ((((float) (180 - this.angle)) / 2f) - 90f)), ((float) ((180f - (((float) (180 - this.angle)) / 2f)) - 90f))) * 0.01745329f;
            float num4 = ((float) (pos.y / this.savedRectSize.y)).Remap(((float) -0.5f), ((float) 0.5f), ((float) ((180f - verticalAngle) / 2f)), ((float) (180f - ((180f - verticalAngle) / 2f)))) * 0.01745329f;
            pos.z = (Mathf.Sin(num4) * Mathf.Cos(f)) * savedRadius;
            pos.y = -savedRadius * Mathf.Cos(num4);
            pos.x = (Mathf.Sin(num4) * Mathf.Sin(f)) * savedRadius;
            if (this.PreserveAspect)
            {
                Vector3* vectorPtr1 = &pos;
                vectorPtr1->z -= savedRadius;
            }
            return pos;
        }

        public void Click()
        {
            if (base.GetComponent<CurvedUIRaycaster>() != null)
            {
                base.GetComponent<CurvedUIRaycaster>().Click();
            }
        }

        public float GetCyllinderRadiusInCanvasSpace()
        {
            float num = !this.PreserveAspect ? ((this.RectTransform.rect.size.x * 0.5f) / Mathf.Sin((Mathf.Clamp((float) this.angle, -180f, 180f) * 0.5f) * 0.01745329f)) : ((this.shape != CurvedUIShape.CYLINDER_VERTICAL) ? (this.RectTransform.rect.size.x / (6.283185f * (((float) this.angle) / 360f))) : (this.RectTransform.rect.size.y / (6.283185f * (((float) this.angle) / 360f))));
            return ((this.angle != 0) ? num : 0f);
        }

        public List<GameObject> GetObjectsHitByRay(Ray ray) => 
            (base.GetComponent<CurvedUIRaycaster>() == null) ? new List<GameObject>() : base.GetComponent<CurvedUIRaycaster>().GetObjectsHitByRay(ray);

        public List<GameObject> GetObjectsUnderPointer() => 
            (base.GetComponent<CurvedUIRaycaster>() == null) ? new List<GameObject>() : base.GetComponent<CurvedUIRaycaster>().GetObjectsUnderPointer();

        public List<GameObject> GetObjectsUnderScreenPos(Vector2 pos, Camera eventCamera = null)
        {
            if (eventCamera == null)
            {
                eventCamera = this.myCanvas.worldCamera;
            }
            return ((base.GetComponent<CurvedUIRaycaster>() == null) ? new List<GameObject>() : base.GetComponent<CurvedUIRaycaster>().GetObjectsUnderScreenPos(pos, eventCamera));
        }

        public Vector2 GetTesslationSize(bool UnmodifiedByQuality = false)
        {
            Vector2 size = this.RectTransform.rect.size;
            float x = size.x;
            float y = size.y;
            if ((this.Angle != 0) || (!this.PreserveAspect && (this.vertAngle != 0)))
            {
                switch (this.shape)
                {
                    case CurvedUIShape.CYLINDER:
                    case CurvedUIShape.RING:
                    case CurvedUIShape.CYLINDER_VERTICAL:
                        x = Mathf.Min((float) (size.x / 4f), (float) (size.x / (Mathf.Abs(this.angle).Remap(0f, 360f, 0f, 1f) * this.baseCircleSegments)));
                        y = Mathf.Min((float) (size.y / 4f), (float) (size.y / (Mathf.Abs(this.angle).Remap(0f, 360f, 0f, 1f) * this.baseCircleSegments)));
                        break;

                    case CurvedUIShape.SPHERE:
                        x = Mathf.Min((float) (size.x / 4f), (float) (size.x / ((Mathf.Abs(this.angle).Remap(0f, 360f, 0f, 1f) * this.baseCircleSegments) * 0.5f)));
                        y = !this.PreserveAspect ? ((this.VerticalAngle != 0) ? (size.y / ((Mathf.Abs(this.VerticalAngle).Remap(0f, 180f, 0f, 1f) * this.baseCircleSegments) * 0.5f)) : 10000f) : ((x * size.y) / size.x);
                        break;

                    default:
                        break;
                }
            }
            return (new Vector2(x, y) / (!UnmodifiedByQuality ? Mathf.Clamp(this.Quality, 0.01f, 10f) : 1f));
        }

        private void OnDisable()
        {
            this.SetAllDirty();
        }

        private void OnEnable()
        {
            this.SetAllDirty();
        }

        public bool RaycastToCanvasSpace(Ray ray, out Vector2 o_positionOnCanvas)
        {
            CurvedUIRaycaster component = base.GetComponent<CurvedUIRaycaster>();
            o_positionOnCanvas = Vector2.zero;
            switch (this.Shape)
            {
                case CurvedUIShape.CYLINDER:
                    return component.RaycastToCyllinderCanvas(ray, out o_positionOnCanvas, true);

                case CurvedUIShape.RING:
                    return component.RaycastToRingCanvas(ray, out o_positionOnCanvas, true);

                case CurvedUIShape.SPHERE:
                    return component.RaycastToSphereCanvas(ray, out o_positionOnCanvas, true);

                case CurvedUIShape.CYLINDER_VERTICAL:
                    return component.RaycastToCyllinderVerticalCanvas(ray, out o_positionOnCanvas, true);
            }
            return false;
        }

        public void SetAllChildrenDirty(bool recalculateCurveOnly = false)
        {
            foreach (CurvedUIVertexEffect effect in base.GetComponentsInChildren<CurvedUIVertexEffect>())
            {
                if (recalculateCurveOnly)
                {
                    effect.SetDirty();
                }
                else
                {
                    effect.CurvingRequired = true;
                }
            }
        }

        public void SetAllDirty()
        {
            foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>())
            {
                graphic.SetAllDirty();
            }
        }

        private void SetUIAngle(int newAngle)
        {
            if (this.myCanvas == null)
            {
                this.myCanvas = base.GetComponent<Canvas>();
            }
            newAngle ??= 1;
            this.angle = newAngle;
            this.savedRadius = this.GetCyllinderRadiusInCanvasSpace();
            foreach (CurvedUIVertexEffect effect in base.GetComponentsInChildren<CurvedUIVertexEffect>())
            {
                effect.TesselationRequired = true;
            }
            foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>())
            {
                graphic.SetVerticesDirty();
            }
            if (Application.isPlaying && (base.GetComponent<CurvedUIRaycaster>() != null))
            {
                base.GetComponent<CurvedUIRaycaster>().RebuildCollider();
            }
        }

        private void Start()
        {
            if (this.myCanvas == null)
            {
                this.myCanvas = base.GetComponent<Canvas>();
            }
            this.savedRadius = this.GetCyllinderRadiusInCanvasSpace();
        }

        private void Update()
        {
            if (this.RectTransform.rect.size != this.savedRectSize)
            {
                this.savedRectSize = this.RectTransform.rect.size;
                this.SetUIAngle(this.angle);
            }
            if ((this.savedRectSize.x == 0f) || (this.savedRectSize.y == 0f))
            {
                Debug.LogError("CurvedUI: Your Canvas size must be bigger than 0!");
            }
        }

        public Vector3 VertexPositionToCurvedCanvas(Vector3 pos)
        {
            switch (this.Shape)
            {
                case CurvedUIShape.CYLINDER:
                    return this.CanvasToCyllinder(pos);

                case CurvedUIShape.RING:
                    return this.CanvasToRing(pos);

                case CurvedUIShape.SPHERE:
                    return this.CanvasToSphere(pos);

                case CurvedUIShape.CYLINDER_VERTICAL:
                    return this.CanvasToCyllinderVertical(pos);
            }
            return Vector3.zero;
        }

        private UnityEngine.RectTransform RectTransform
        {
            get
            {
                if (this.m_rectTransform == null)
                {
                    this.m_rectTransform = base.transform as UnityEngine.RectTransform;
                }
                return this.m_rectTransform;
            }
        }

        public int BaseCircleSegments =>
            this.baseCircleSegments;

        public int Angle
        {
            get => 
                this.angle;
            set
            {
                if (this.angle != value)
                {
                    this.SetUIAngle(value);
                }
            }
        }

        public float Quality
        {
            get => 
                this.quality;
            set
            {
                if (this.quality != value)
                {
                    this.quality = value;
                    this.SetUIAngle(this.angle);
                }
            }
        }

        public CurvedUIShape Shape
        {
            get => 
                this.shape;
            set
            {
                if (this.shape != value)
                {
                    this.shape = value;
                    this.SetUIAngle(this.angle);
                }
            }
        }

        public int VerticalAngle
        {
            get => 
                this.vertAngle;
            set
            {
                if (this.vertAngle != value)
                {
                    this.vertAngle = value;
                    this.SetUIAngle(this.angle);
                }
            }
        }

        public float RingFill
        {
            get => 
                this.ringFill;
            set
            {
                if (this.ringFill != value)
                {
                    this.ringFill = value;
                    this.SetUIAngle(this.angle);
                }
            }
        }

        public float SavedRadius
        {
            get
            {
                if (this.savedRadius == 0f)
                {
                    this.savedRadius = this.GetCyllinderRadiusInCanvasSpace();
                }
                return this.savedRadius;
            }
        }

        public int RingExternalDiameter
        {
            get => 
                this.ringExternalDiamater;
            set
            {
                if (this.ringExternalDiamater != value)
                {
                    this.ringExternalDiamater = value;
                    this.SetUIAngle(this.angle);
                }
            }
        }

        public bool RingFlipVertical
        {
            get => 
                this.ringFlipVertical;
            set
            {
                if (this.ringFlipVertical != value)
                {
                    this.ringFlipVertical = value;
                    this.SetUIAngle(this.angle);
                }
            }
        }

        public bool PreserveAspect
        {
            get => 
                this.preserveAspect;
            set
            {
                if (this.preserveAspect != value)
                {
                    this.preserveAspect = value;
                    this.SetUIAngle(this.angle);
                }
            }
        }

        public bool Interactable
        {
            get => 
                this.interactable;
            set => 
                this.interactable = value;
        }

        public bool ForceUseBoxCollider
        {
            get => 
                this.forceUseBoxCollider;
            set => 
                this.forceUseBoxCollider = value;
        }

        public bool BlocksRaycasts
        {
            get => 
                this.blocksRaycasts;
            set
            {
                if (this.blocksRaycasts != value)
                {
                    this.blocksRaycasts = value;
                    if (Application.isPlaying && (base.GetComponent<CurvedUIRaycaster>() != null))
                    {
                        base.GetComponent<CurvedUIRaycaster>().RebuildCollider();
                    }
                }
            }
        }

        public bool RaycastMyLayerOnly
        {
            get => 
                this.raycastMyLayerOnly;
            set => 
                this.raycastMyLayerOnly = value;
        }

        public CurvedUIInputModule.CUIControlMethod ControlMethod
        {
            get => 
                CurvedUIInputModule.ControlMethod;
            set => 
                CurvedUIInputModule.ControlMethod = value;
        }

        public bool GazeUseTimedClick
        {
            get => 
                CurvedUIInputModule.Instance.GazeUseTimedClick;
            set => 
                CurvedUIInputModule.Instance.GazeUseTimedClick = value;
        }

        public float GazeClickTimer
        {
            get => 
                CurvedUIInputModule.Instance.GazeClickTimer;
            set => 
                CurvedUIInputModule.Instance.GazeClickTimer = value;
        }

        public float GazeClickTimerDelay
        {
            get => 
                CurvedUIInputModule.Instance.GazeClickTimerDelay;
            set => 
                CurvedUIInputModule.Instance.GazeClickTimerDelay = value;
        }

        public float GazeTimerProgress =>
            CurvedUIInputModule.Instance.GazeTimerProgress;

        public enum CurvedUIShape
        {
            CYLINDER,
            RING,
            SPHERE,
            CYLINDER_VERTICAL
        }
    }
}

