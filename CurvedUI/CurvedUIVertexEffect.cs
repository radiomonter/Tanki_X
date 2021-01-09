namespace CurvedUI
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class CurvedUIVertexEffect : BaseMeshEffect
    {
        [Tooltip("Check to skip tesselation pass on this object. CurvedUI will not create additional vertices to make this object have a smoother curve. Checking this can solve some issues if you create your own procedural mesh for this object. Default false.")]
        public bool DoNotTesselate;
        private Canvas myCanvas;
        private CurvedUISettings mySettings;
        private Graphic myGraphic;
        private Image myImage;
        private Text myText;
        private TextMeshProUGUI myTMP;
        private CurvedUITMPSubmesh myTMPSubMesh;
        private bool tesselationRequired = true;
        private bool curvingRequired = true;
        private float angle = 90f;
        private bool TransformMisaligned;
        private Matrix4x4 CanvasToWorld;
        private Matrix4x4 CanvasToLocal;
        private Matrix4x4 MyToWorld;
        private Matrix4x4 MyToLocal;
        private VertexHelper SavedVertexHelper;
        private List<UIVertex> SavedVerteees;
        private List<UIVertex> tesselatedVerts;
        [SerializeField, HideInInspector]
        private Vector3 savedPos;
        [SerializeField, HideInInspector]
        private Vector3 savedUp;
        [SerializeField, HideInInspector]
        private Vector2 savedRectSize;
        [SerializeField, HideInInspector]
        private Color savedColor;
        [SerializeField, HideInInspector]
        private Vector2 savedTextUV0;
        [SerializeField, HideInInspector]
        private float savedFill;

        private void CheckTextFontMaterial()
        {
            if ((this.myText && (this.myText.cachedTextGenerator.verts.Count > 0)) && (this.myText.cachedTextGenerator.verts[0].uv0 != this.savedTextUV0))
            {
                this.savedTextUV0 = this.myText.cachedTextGenerator.verts[0].uv0;
                this.tesselationRequired = true;
            }
        }

        private unsafe UIVertex CurveVertex(UIVertex input, float cylinder_angle, float radius, Vector2 canvasSize)
        {
            Vector3 position = input.position;
            position = this.CanvasToLocal.MultiplyPoint3x4(this.MyToWorld.MultiplyPoint3x4(position));
            if ((this.mySettings.Shape == CurvedUISettings.CurvedUIShape.CYLINDER) && (this.mySettings.Angle != 0))
            {
                float f = ((position.x / canvasSize.x) * cylinder_angle) * 0.01745329f;
                radius += position.z;
                position.x = Mathf.Sin(f) * radius;
                Vector3* vectorPtr1 = &position;
                vectorPtr1->z += (Mathf.Cos(f) * radius) - radius;
            }
            else if ((this.mySettings.Shape == CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL) && (this.mySettings.Angle != 0))
            {
                float f = ((position.y / canvasSize.y) * cylinder_angle) * 0.01745329f;
                radius += position.z;
                position.y = Mathf.Sin(f) * radius;
                Vector3* vectorPtr2 = &position;
                vectorPtr2->z += (Mathf.Cos(f) * radius) - radius;
            }
            else if (this.mySettings.Shape == CurvedUISettings.CurvedUIShape.RING)
            {
                float num3 = 0f;
                float num4 = position.y.Remap((canvasSize.y * 0.5f) * (!this.mySettings.RingFlipVertical ? ((float) (-1)) : ((float) 1)), (-canvasSize.y * 0.5f) * (!this.mySettings.RingFlipVertical ? ((float) (-1)) : ((float) 1)), (this.mySettings.RingExternalDiameter * (1f - this.mySettings.RingFill)) * 0.5f, this.mySettings.RingExternalDiameter * 0.5f);
                float f = ((float) (position.x / canvasSize.x)).Remap(((float) -0.5f), ((float) 0.5f), ((float) 1.570796f), ((float) ((cylinder_angle * 0.01745329f) + 1.570796f))) - num3;
                position.x = num4 * Mathf.Cos(f);
                position.y = num4 * Mathf.Sin(f);
            }
            else if ((this.mySettings.Shape == CurvedUISettings.CurvedUIShape.SPHERE) && (this.mySettings.Angle != 0))
            {
                float verticalAngle = this.mySettings.VerticalAngle;
                float num7 = -position.z;
                if (this.mySettings.PreserveAspect)
                {
                    verticalAngle = cylinder_angle * (canvasSize.y / canvasSize.x);
                }
                else
                {
                    radius = canvasSize.x / 2f;
                    if (verticalAngle == 0f)
                    {
                        return input;
                    }
                }
                float f = ((float) (position.x / canvasSize.x)).Remap(((float) -0.5f), ((float) 0.5f), ((float) (((180f - cylinder_angle) / 2f) - 90f)), ((float) ((180f - ((180f - cylinder_angle) / 2f)) - 90f))) * 0.01745329f;
                float num9 = ((float) (position.y / canvasSize.y)).Remap(((float) -0.5f), ((float) 0.5f), ((float) ((180f - verticalAngle) / 2f)), ((float) (180f - ((180f - verticalAngle) / 2f)))) * 0.01745329f;
                position.z = (Mathf.Sin(num9) * Mathf.Cos(f)) * (radius + num7);
                position.y = -(radius + num7) * Mathf.Cos(num9);
                position.x = (Mathf.Sin(num9) * Mathf.Sin(f)) * (radius + num7);
                if (this.mySettings.PreserveAspect)
                {
                    Vector3* vectorPtr3 = &position;
                    vectorPtr3->z -= radius;
                }
            }
            input.position = this.MyToLocal.MultiplyPoint3x4(this.CanvasToWorld.MultiplyPoint3x4(position));
            return input;
        }

        public CurvedUISettings FindParentSettings(bool forceNew = false)
        {
            if ((this.mySettings == null) || forceNew)
            {
                this.mySettings = base.GetComponentInParent<CurvedUISettings>();
                if (this.mySettings == null)
                {
                    return null;
                }
                this.myCanvas = this.mySettings.GetComponent<Canvas>();
                this.angle = this.mySettings.Angle;
                this.myImage = base.GetComponent<Image>();
            }
            return this.mySettings;
        }

        private void FontTextureRebuiltCallback(Font fontie)
        {
            if (this.myText.font == fontie)
            {
                this.tesselationRequired = true;
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (this.IsActive())
            {
                if (this.mySettings == null)
                {
                    this.FindParentSettings(false);
                }
                if ((this.mySettings != null) && this.mySettings.enabled)
                {
                    this.CheckTextFontMaterial();
                    if (this.tesselationRequired || (this.curvingRequired || ((this.SavedVertexHelper == null) || (this.SavedVertexHelper.currentVertCount == 0))))
                    {
                        this.SavedVerteees = new List<UIVertex>();
                        vh.GetUIVertexStream(this.SavedVerteees);
                        this.ModifyVerts(this.SavedVerteees);
                        if (this.SavedVertexHelper == null)
                        {
                            this.SavedVertexHelper = new VertexHelper();
                        }
                        else
                        {
                            this.SavedVertexHelper.Clear();
                        }
                        if ((this.SavedVerteees.Count % 4) != 0)
                        {
                            this.SavedVertexHelper.AddUIVertexTriangleStream(this.SavedVerteees);
                        }
                        else
                        {
                            for (int i = 0; i < this.SavedVerteees.Count; i += 4)
                            {
                                UIVertex[] verts = new UIVertex[] { this.SavedVerteees[i], this.SavedVerteees[i + 1], this.SavedVerteees[i + 2], this.SavedVerteees[i + 3] };
                                this.SavedVertexHelper.AddUIVertexQuad(verts);
                            }
                        }
                        this.SavedVertexHelper.GetUIVertexStream(this.SavedVerteees);
                        this.curvingRequired = false;
                    }
                    vh.Clear();
                    vh.AddUIVertexTriangleStream(this.SavedVerteees);
                }
            }
        }

        private void ModifyQuad(List<UIVertex> verts, int vertexIndex, Vector2 requiredSize)
        {
            UIVertex[] quad = new UIVertex[4];
            for (int i = 0; i < 4; i++)
            {
                quad[i] = verts[vertexIndex + i];
            }
            Vector3 vector = quad[2].position - quad[1].position;
            Vector3 vector2 = quad[1].position - quad[0].position;
            if ((this.myImage != null) && (this.myImage.type == Image.Type.Filled))
            {
                vector = (vector.x <= (quad[3].position - quad[0].position).x) ? (quad[3].position - quad[0].position) : vector;
                vector2 = (vector2.y <= (quad[2].position - quad[3].position).y) ? (quad[2].position - quad[3].position) : vector2;
            }
            int num2 = 1;
            int num3 = 1;
            if (this.TransformMisaligned || ((this.mySettings.Shape == CurvedUISettings.CurvedUIShape.SPHERE) || (this.mySettings.Shape == CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL)))
            {
                num3 = Mathf.CeilToInt(vector2.magnitude * (1f / Mathf.Max(1f, requiredSize.y)));
            }
            if (this.TransformMisaligned || (this.mySettings.Shape != CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL))
            {
                num2 = Mathf.CeilToInt(vector.magnitude * (1f / Mathf.Max(1f, requiredSize.x)));
            }
            bool flag = false;
            bool flag2 = false;
            float y = 0f;
            int num5 = 0;
            while ((num5 < num3) || !flag)
            {
                flag = true;
                float num6 = (num5 + 1f) / ((float) num3);
                float x = 0f;
                int num8 = 0;
                while (true)
                {
                    if ((num8 >= num2) && flag2)
                    {
                        y = num6;
                        num5++;
                        break;
                    }
                    flag2 = true;
                    float num9 = (num8 + 1f) / ((float) num2);
                    verts.Add(this.TesselateQuad(quad, x, y));
                    verts.Add(this.TesselateQuad(quad, x, num6));
                    verts.Add(this.TesselateQuad(quad, num9, num6));
                    verts.Add(this.TesselateQuad(quad, num9, y));
                    x = num9;
                    num8++;
                }
            }
        }

        private void ModifyVerts(List<UIVertex> verts)
        {
            if ((verts != null) && (verts.Count != 0))
            {
                this.CanvasToWorld = this.myCanvas.transform.localToWorldMatrix;
                this.CanvasToLocal = this.myCanvas.transform.worldToLocalMatrix;
                this.MyToWorld = base.transform.localToWorldMatrix;
                this.MyToLocal = base.transform.worldToLocalMatrix;
                if (this.tesselationRequired || !Application.isPlaying)
                {
                    this.TesselateGeometry(verts);
                    this.tesselatedVerts = new List<UIVertex>(verts);
                    this.savedRectSize = (base.transform as RectTransform).rect.size;
                    this.tesselationRequired = false;
                }
                this.angle = this.mySettings.Angle;
                float cyllinderRadiusInCanvasSpace = this.mySettings.GetCyllinderRadiusInCanvasSpace();
                Vector2 size = (this.myCanvas.transform as RectTransform).rect.size;
                int count = verts.Count;
                if (this.tesselatedVerts != null)
                {
                    UIVertex[] collection = new UIVertex[this.tesselatedVerts.Count];
                    int index = 0;
                    while (true)
                    {
                        if (index >= this.tesselatedVerts.Count)
                        {
                            verts.AddRange(collection);
                            verts.RemoveRange(0, count);
                            break;
                        }
                        collection[index] = this.CurveVertex(this.tesselatedVerts[index], this.angle, cyllinderRadiusInCanvasSpace, size);
                        index++;
                    }
                }
                else
                {
                    UIVertex[] collection = new UIVertex[verts.Count];
                    int index = 0;
                    while (true)
                    {
                        if (index >= count)
                        {
                            verts.AddRange(collection);
                            verts.RemoveRange(0, count);
                            break;
                        }
                        collection[index] = this.CurveVertex(verts[index], this.angle, cyllinderRadiusInCanvasSpace, size);
                        index++;
                    }
                }
            }
        }

        protected override void OnDisable()
        {
            if (this.myGraphic)
            {
                this.myGraphic.UnregisterDirtyMaterialCallback(new UnityAction(this.TesselationRequiredCallback));
            }
            if (this.myText)
            {
                this.myText.UnregisterDirtyVerticesCallback(new UnityAction(this.TesselationRequiredCallback));
                Font.textureRebuilt -= new Action<Font>(this.FontTextureRebuiltCallback);
            }
        }

        protected override void OnEnable()
        {
            this.FindParentSettings(false);
            this.myGraphic = base.GetComponent<Graphic>();
            if (this.myGraphic)
            {
                this.myGraphic.RegisterDirtyMaterialCallback(new UnityAction(this.TesselationRequiredCallback));
                this.myGraphic.SetVerticesDirty();
            }
            this.myText = base.GetComponent<Text>();
            if (this.myText)
            {
                this.myText.RegisterDirtyVerticesCallback(new UnityAction(this.TesselationRequiredCallback));
                Font.textureRebuilt += new Action<Font>(this.FontTextureRebuiltCallback);
            }
            this.myTMP = base.GetComponent<TextMeshProUGUI>();
            this.myTMPSubMesh = base.GetComponent<CurvedUITMPSubmesh>();
        }

        public void SetDirty()
        {
            this.TesselationRequired = true;
        }

        private void TesselateGeometry(List<UIVertex> verts)
        {
            Vector2 tesslationSize = this.mySettings.GetTesslationSize(false);
            this.TransformMisaligned = !this.savedUp.AlmostEqual(Vector3.up.normalized, 0.01);
            this.TrisToQuads(verts);
            if ((this.myText == null) && ((this.myTMP == null) && !this.DoNotTesselate))
            {
                int count = verts.Count;
                int vertexIndex = 0;
                while (true)
                {
                    if (vertexIndex >= count)
                    {
                        verts.RemoveRange(0, count);
                        break;
                    }
                    this.ModifyQuad(verts, vertexIndex, tesslationSize);
                    vertexIndex += 4;
                }
            }
        }

        private UIVertex TesselateQuad(UIVertex[] quad, float x, float y)
        {
            UIVertex vertex = new UIVertex();
            float[] numArray = new float[] { (1f - x) * (1f - y), (1f - x) * y, x * y, x * (1f - y) };
            Vector2 zero = Vector2.zero;
            Vector2 vector2 = Vector2.zero;
            Vector3 vector3 = Vector3.zero;
            for (int i = 0; i < 4; i++)
            {
                zero += quad[i].uv0 * numArray[i];
                vector2 += quad[i].uv1 * numArray[i];
                vector3 += quad[i].position * numArray[i];
            }
            vertex.position = vector3;
            vertex.color = quad[0].color;
            vertex.uv0 = zero;
            vertex.uv1 = vector2;
            vertex.normal = quad[0].normal;
            vertex.tangent = quad[0].tangent;
            return vertex;
        }

        private void TesselationRequiredCallback()
        {
            this.tesselationRequired = true;
        }

        private void TrisToQuads(List<UIVertex> verts)
        {
            int num = 0;
            int count = verts.Count;
            UIVertex[] collection = new UIVertex[(count / 6) * 4];
            for (int i = 0; i < count; i += 6)
            {
                collection[num++] = verts[i];
                collection[num++] = verts[i + 1];
                collection[num++] = verts[i + 2];
                collection[num++] = verts[i + 4];
            }
            verts.AddRange(collection);
            verts.RemoveRange(0, count);
        }

        private void Update()
        {
            if (!this.myTMP && !this.myTMPSubMesh)
            {
                if (!this.tesselationRequired)
                {
                    if ((base.transform as RectTransform).rect.size != this.savedRectSize)
                    {
                        this.tesselationRequired = true;
                    }
                    else if (this.myGraphic != null)
                    {
                        if (this.myGraphic.color != this.savedColor)
                        {
                            this.tesselationRequired = true;
                            this.savedColor = this.myGraphic.color;
                        }
                        else if ((this.myImage != null) && (this.myImage.fillAmount != this.savedFill))
                        {
                            this.tesselationRequired = true;
                            this.savedFill = this.myImage.fillAmount;
                        }
                    }
                }
                if (!this.tesselationRequired && !this.curvingRequired)
                {
                    Vector3 a = this.mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
                    if (!a.AlmostEqual(this.savedPos, 0.01) && ((this.mySettings.Shape != CurvedUISettings.CurvedUIShape.CYLINDER) || ((Mathf.Pow(a.x - this.savedPos.x, 2f) > 1E-05) || (Mathf.Pow(a.z - this.savedPos.z, 2f) > 1E-05))))
                    {
                        this.savedPos = a;
                        this.curvingRequired = true;
                    }
                    Vector3 normalized = this.mySettings.transform.worldToLocalMatrix.MultiplyVector(base.transform.up).normalized;
                    if (!this.savedUp.AlmostEqual(normalized, 0.0001))
                    {
                        bool flag = normalized.AlmostEqual(Vector3.up.normalized, 0.01);
                        bool flag2 = this.savedUp.AlmostEqual(Vector3.up.normalized, 0.01);
                        if ((!flag && flag2) || (flag && !flag2))
                        {
                            this.tesselationRequired = true;
                        }
                        this.savedUp = normalized;
                        this.curvingRequired = true;
                    }
                }
                if (this.myGraphic && (this.tesselationRequired || this.curvingRequired))
                {
                    this.myGraphic.SetVerticesDirty();
                }
            }
        }

        public bool TesselationRequired
        {
            get => 
                this.tesselationRequired;
            set => 
                this.tesselationRequired = value;
        }

        public bool CurvingRequired
        {
            get => 
                this.curvingRequired;
            set => 
                this.curvingRequired = value;
        }
    }
}

