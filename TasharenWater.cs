using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Renderer)), AddComponentMenu("Tasharen/Water")]
public class TasharenWater : MonoBehaviour
{
    public static TasharenWater instance;
    public Quality quality = Quality.High;
    public LayerMask highReflectionMask = -1;
    public LayerMask mediumReflectionMask = -1;
    public bool keepUnderCamera = true;
    public bool automaticQuality = true;
    private Transform mTrans;
    private Hashtable mCameras = new Hashtable();
    private RenderTexture mTex;
    private int mTexSize;
    private Renderer mRen;
    private Color mSpecular;
    private bool mDepthTexSupport;
    private bool mStreamingWater;
    private static bool mIsRendering = false;
    private static Vector3 mTemp = ((Vector3) Vector4.one);
    [NonSerialized]
    private Vector4 mReflectionPlane;
    [NonSerialized]
    private Texture2D mDepthTex;
    [NonSerialized]
    private bool mDepthTexIsValid;

    private void Awake()
    {
        this.mTrans = base.transform;
        this.mRen = base.GetComponent<Renderer>();
        this.mSpecular = (Color) new Color32(0x93, 0x93, 0x93, 0xff);
        this.mDepthTexSupport = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
    }

    private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
    {
        mTemp.x = SignExt(clipPlane.x);
        mTemp.y = SignExt(clipPlane.y);
        Vector4 b = (Vector4) (projection.inverse * mTemp);
        Vector4 vector2 = clipPlane * (2f / Vector4.Dot(clipPlane, b));
        projection[2] = vector2.x - projection[3];
        projection[6] = vector2.y - projection[7];
        projection[10] = vector2.z - projection[11];
        projection[14] = vector2.w - projection[15];
    }

    private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
    {
        reflectionMat.m00 = 1f - ((2f * plane[0]) * plane[0]);
        reflectionMat.m01 = (-2f * plane[0]) * plane[1];
        reflectionMat.m02 = (-2f * plane[0]) * plane[2];
        reflectionMat.m03 = (-2f * plane[3]) * plane[0];
        reflectionMat.m10 = (-2f * plane[1]) * plane[0];
        reflectionMat.m11 = 1f - ((2f * plane[1]) * plane[1]);
        reflectionMat.m12 = (-2f * plane[1]) * plane[2];
        reflectionMat.m13 = (-2f * plane[3]) * plane[1];
        reflectionMat.m20 = (-2f * plane[2]) * plane[0];
        reflectionMat.m21 = (-2f * plane[2]) * plane[1];
        reflectionMat.m22 = 1f - ((2f * plane[2]) * plane[2]);
        reflectionMat.m23 = (-2f * plane[3]) * plane[2];
        reflectionMat.m30 = 0f;
        reflectionMat.m31 = 0f;
        reflectionMat.m32 = 0f;
        reflectionMat.m33 = 1f;
    }

    private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
    {
        Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
        Vector3 lhs = worldToCameraMatrix.MultiplyPoint(pos);
        Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
    }

    private void Clear()
    {
        if (this.mTex)
        {
            DestroyImmediate(this.mTex);
            this.mTex = null;
        }
    }

    private void CopyCamera(Camera src, Camera dest)
    {
        dest.clearFlags = src.clearFlags;
        dest.backgroundColor = src.backgroundColor;
        dest.farClipPlane = src.farClipPlane;
        dest.nearClipPlane = src.nearClipPlane;
        dest.orthographic = src.orthographic;
        dest.fieldOfView = src.fieldOfView;
        dest.aspect = src.aspect;
        dest.orthographicSize = src.orthographicSize;
        dest.depthTextureMode = DepthTextureMode.None;
        dest.renderingPath = RenderingPath.Forward;
    }

    public static Quality GetQuality() => 
        (Quality) PlayerPrefs.GetInt("Water", 3);

    private Camera GetReflectionCamera(Camera current, Material mat, int textureSize)
    {
        if (!this.mTex || (this.mTexSize != textureSize))
        {
            if (this.mTex)
            {
                DestroyImmediate(this.mTex);
            }
            this.mTex = new RenderTexture(textureSize, textureSize, 0x10);
            this.mTex.name = "__MirrorReflection" + base.GetInstanceID();
            this.mTex.isPowerOfTwo = true;
            this.mTex.hideFlags = HideFlags.DontSave;
            this.mTexSize = textureSize;
        }
        Camera component = this.mCameras[current] as Camera;
        if (!component)
        {
            object[] objArray1 = new object[] { "Mirror Refl Camera id", base.GetInstanceID(), " for ", current.GetInstanceID() };
            Type[] components = new Type[] { typeof(Camera), typeof(Skybox) };
            component = new GameObject(string.Concat(objArray1), components) { hideFlags = HideFlags.HideAndDontSave }.GetComponent<Camera>();
            component.enabled = false;
            Transform transform = component.transform;
            transform.position = this.mTrans.position;
            transform.rotation = this.mTrans.rotation;
            component.gameObject.AddComponent<FlareLayer>();
            this.mCameras[current] = component;
        }
        if (mat.HasProperty("_ReflectionTex"))
        {
            mat.SetTexture("_ReflectionTex", this.mTex);
        }
        return component;
    }

    private void LateUpdate()
    {
        if (this.keepUnderCamera)
        {
            Camera main = Camera.main;
            if (main != null)
            {
                Vector3 position = main.transform.position;
                position.y = this.mTrans.position.y;
                if (this.mTrans.position != position)
                {
                    this.mTrans.position = position;
                }
            }
        }
    }

    private void OnDisable()
    {
        this.Clear();
        IDictionaryEnumerator enumerator = this.mCameras.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                DestroyImmediate(((Camera) current.Value).gameObject);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        this.mCameras.Clear();
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnEnable()
    {
        instance = this;
        this.mStreamingWater = PlayerPrefs.GetInt("Streaming Water", 0) == 1;
        if (this.automaticQuality)
        {
            this.quality = GetQuality();
        }
    }

    private void OnWillRenderObject()
    {
        if (!mIsRendering)
        {
            if (!base.enabled || (!this.mRen || !this.mRen.enabled))
            {
                this.Clear();
            }
            else
            {
                Material sharedMaterial = this.mRen.sharedMaterial;
                if (sharedMaterial)
                {
                    Camera current = Camera.current;
                    if (current)
                    {
                        if (this.mStreamingWater)
                        {
                            sharedMaterial.SetColor("_Specular", Color.black);
                        }
                        else
                        {
                            sharedMaterial.SetColor("_Specular", this.mSpecular);
                        }
                        if (!this.mDepthTexSupport)
                        {
                            this.quality = Quality.Fastest;
                        }
                        if (this.quality != Quality.Fastest)
                        {
                            if (this.quality == Quality.Low)
                            {
                                sharedMaterial.shader.maximumLOD = 200;
                                this.Clear();
                            }
                            else
                            {
                                current.depthTextureMode |= DepthTextureMode.Depth;
                                LayerMask reflectionMask = this.reflectionMask;
                                int reflectionTextureSize = this.reflectionTextureSize;
                                if ((reflectionMask == 0) || (reflectionTextureSize < 0x200))
                                {
                                    sharedMaterial.shader.maximumLOD = 300;
                                    this.Clear();
                                }
                                else
                                {
                                    sharedMaterial.shader.maximumLOD = 400;
                                    mIsRendering = true;
                                    Camera dest = this.GetReflectionCamera(current, sharedMaterial, reflectionTextureSize);
                                    Vector3 position = this.mTrans.position;
                                    Vector3 up = this.mTrans.up;
                                    this.CopyCamera(current, dest);
                                    float num12 = -Vector3.Dot(up, position);
                                    this.mReflectionPlane.x = up.x;
                                    this.mReflectionPlane.y = up.y;
                                    this.mReflectionPlane.z = up.z;
                                    this.mReflectionPlane.w = num12;
                                    Matrix4x4 zero = Matrix4x4.zero;
                                    CalculateReflectionMatrix(ref zero, this.mReflectionPlane);
                                    Vector3 v = current.transform.position;
                                    Vector3 vector5 = zero.MultiplyPoint(v);
                                    dest.worldToCameraMatrix = current.worldToCameraMatrix * zero;
                                    Matrix4x4 projectionMatrix = current.projectionMatrix;
                                    CalculateObliqueMatrix(ref projectionMatrix, this.CameraSpacePlane(dest, position, up, 1f));
                                    dest.projectionMatrix = projectionMatrix;
                                    dest.cullingMask = -17 & reflectionMask.value;
                                    dest.targetTexture = this.mTex;
                                    GL.SetRevertBackfacing(true);
                                    dest.transform.position = vector5;
                                    Vector3 eulerAngles = current.transform.eulerAngles;
                                    eulerAngles.x = 0f;
                                    dest.transform.eulerAngles = eulerAngles;
                                    dest.Render();
                                    dest.transform.position = v;
                                    GL.SetRevertBackfacing(false);
                                    mIsRendering = false;
                                }
                            }
                        }
                        else
                        {
                            sharedMaterial.shader.maximumLOD = 100;
                            int width = 0x100;
                            float num2 = width * 0.5f;
                            sharedMaterial.SetFloat("_InvScale", 1f / ((float) width));
                            Terrain activeTerrain = Terrain.activeTerrain;
                            float num3 = (activeTerrain == null) ? 0f : activeTerrain.transform.position.y;
                            if (activeTerrain != null)
                            {
                                if (this.mDepthTex == null)
                                {
                                    this.mDepthTexIsValid = false;
                                    this.mDepthTex = new Texture2D(width, width, TextureFormat.Alpha8, false);
                                }
                                if (!this.mDepthTexIsValid)
                                {
                                    this.mDepthTexIsValid = true;
                                    Color32[] colors = new Color32[width * width];
                                    float num4 = ((float) (width + 1)) / ((float) width);
                                    int num5 = 0;
                                    while (true)
                                    {
                                        if (num5 >= width)
                                        {
                                            this.mDepthTex.SetPixels32(colors);
                                            this.mDepthTex.wrapMode = TextureWrapMode.Clamp;
                                            this.mDepthTex.Apply();
                                            break;
                                        }
                                        float z = -num2 + (num5 * num4);
                                        int num7 = 0;
                                        while (true)
                                        {
                                            if (num7 >= width)
                                            {
                                                num5++;
                                                break;
                                            }
                                            float x = -num2 + (num7 * num4);
                                            float num9 = activeTerrain.SampleHeight(new Vector3(x, 0f, z)) + num3;
                                            if (num9 < 0f)
                                            {
                                                colors[num7 + (num5 * width)].a = (byte) Mathf.RoundToInt(255f * Mathf.Clamp01(-num9 * 0.125f));
                                            }
                                            else
                                            {
                                                num9 = colors[num7 + (num5 * width)].a = 0;
                                            }
                                            num7++;
                                        }
                                    }
                                }
                            }
                            sharedMaterial.SetTexture("_DepthTex", this.mDepthTex);
                        }
                    }
                }
            }
        }
    }

    public static void SetQuality(Quality q)
    {
        TasharenWater[] waterArray = FindObjectsOfType(typeof(TasharenWater)) as TasharenWater[];
        if (waterArray.Length <= 0)
        {
            PlayerPrefs.SetInt("Water", (int) q);
        }
        else
        {
            foreach (TasharenWater water in waterArray)
            {
                water.quality = q;
            }
        }
    }

    private static float SignExt(float a) => 
        (a <= 0f) ? ((a >= 0f) ? 0f : -1f) : 1f;

    public bool depthTextureSupport =>
        this.mDepthTexSupport;

    public int reflectionTextureSize
    {
        get
        {
            Quality quality = this.quality;
            return ((quality == Quality.Uber) ? 0x400 : (((quality == Quality.High) || (quality == Quality.Medium)) ? 0x200 : 0));
        }
    }

    public LayerMask reflectionMask
    {
        get
        {
            Quality quality = this.quality;
            return (((quality == Quality.Uber) || (quality == Quality.High)) ? this.highReflectionMask : ((quality == Quality.Medium) ? this.mediumReflectionMask : 0));
        }
    }

    public bool useRefraction =>
        this.quality > Quality.Fastest;

    public enum Quality
    {
        Fastest,
        Low,
        Medium,
        High,
        Uber
    }
}

