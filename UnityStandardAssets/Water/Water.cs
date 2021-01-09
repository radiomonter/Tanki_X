namespace UnityStandardAssets.Water
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class Water : MonoBehaviour
    {
        public WaterMode waterMode = WaterMode.Refractive;
        public bool disablePixelLights = true;
        public int textureSize = 0x100;
        public float clipPlaneOffset = 0.07f;
        public LayerMask reflectLayers = -1;
        public LayerMask refractLayers = -1;
        private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();
        private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();
        private RenderTexture m_ReflectionTexture;
        private RenderTexture m_RefractionTexture;
        private WaterMode m_HardwareWaterSupport = WaterMode.Refractive;
        private int m_OldReflectionTextureSize;
        private int m_OldRefractionTextureSize;
        private static bool s_InsideWater;

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
            Vector3 lhs = worldToCameraMatrix.MultiplyPoint(pos + (normal * this.clipPlaneOffset));
            Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
        }

        private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
        {
            WaterMode waterMode = this.GetWaterMode();
            reflectionCamera = null;
            refractionCamera = null;
            if (waterMode >= WaterMode.Reflective)
            {
                if (!this.m_ReflectionTexture || (this.m_OldReflectionTextureSize != this.textureSize))
                {
                    if (this.m_ReflectionTexture)
                    {
                        DestroyImmediate(this.m_ReflectionTexture);
                    }
                    this.m_ReflectionTexture = new RenderTexture(this.textureSize, this.textureSize, 0x10);
                    this.m_ReflectionTexture.name = "__WaterReflection" + base.GetInstanceID();
                    this.m_ReflectionTexture.isPowerOfTwo = true;
                    this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
                    this.m_OldReflectionTextureSize = this.textureSize;
                }
                this.m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
                if (!reflectionCamera)
                {
                    object[] objArray1 = new object[] { "Water Refl Camera id", base.GetInstanceID(), " for ", currentCamera.GetInstanceID() };
                    Type[] components = new Type[] { typeof(Camera), typeof(Skybox) };
                    GameObject obj2 = new GameObject(string.Concat(objArray1), components);
                    reflectionCamera = obj2.GetComponent<Camera>();
                    reflectionCamera.enabled = false;
                    reflectionCamera.transform.position = base.transform.position;
                    reflectionCamera.transform.rotation = base.transform.rotation;
                    reflectionCamera.gameObject.AddComponent<FlareLayer>();
                    obj2.hideFlags = HideFlags.HideAndDontSave;
                    this.m_ReflectionCameras[currentCamera] = reflectionCamera;
                }
            }
            if (waterMode >= WaterMode.Refractive)
            {
                if (!this.m_RefractionTexture || (this.m_OldRefractionTextureSize != this.textureSize))
                {
                    if (this.m_RefractionTexture)
                    {
                        DestroyImmediate(this.m_RefractionTexture);
                    }
                    this.m_RefractionTexture = new RenderTexture(this.textureSize, this.textureSize, 0x10);
                    this.m_RefractionTexture.name = "__WaterRefraction" + base.GetInstanceID();
                    this.m_RefractionTexture.isPowerOfTwo = true;
                    this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
                    this.m_OldRefractionTextureSize = this.textureSize;
                }
                this.m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
                if (!refractionCamera)
                {
                    object[] objArray2 = new object[] { "Water Refr Camera id", base.GetInstanceID(), " for ", currentCamera.GetInstanceID() };
                    Type[] components = new Type[] { typeof(Camera), typeof(Skybox) };
                    GameObject obj3 = new GameObject(string.Concat(objArray2), components);
                    refractionCamera = obj3.GetComponent<Camera>();
                    refractionCamera.enabled = false;
                    refractionCamera.transform.position = base.transform.position;
                    refractionCamera.transform.rotation = base.transform.rotation;
                    refractionCamera.gameObject.AddComponent<FlareLayer>();
                    obj3.hideFlags = HideFlags.HideAndDontSave;
                    this.m_RefractionCameras[currentCamera] = refractionCamera;
                }
            }
        }

        private WaterMode FindHardwareWaterSupport()
        {
            if (!SystemInfo.supportsRenderTextures || !base.GetComponent<Renderer>())
            {
                return WaterMode.Simple;
            }
            Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
            if (!sharedMaterial)
            {
                return WaterMode.Simple;
            }
            string tag = sharedMaterial.GetTag("WATERMODE", false);
            return ((tag != "Refractive") ? ((tag != "Reflective") ? WaterMode.Simple : WaterMode.Reflective) : WaterMode.Refractive);
        }

        private WaterMode GetWaterMode() => 
            (this.m_HardwareWaterSupport >= this.waterMode) ? this.waterMode : this.m_HardwareWaterSupport;

        private void OnDisable()
        {
            if (this.m_ReflectionTexture)
            {
                DestroyImmediate(this.m_ReflectionTexture);
                this.m_ReflectionTexture = null;
            }
            if (this.m_RefractionTexture)
            {
                DestroyImmediate(this.m_RefractionTexture);
                this.m_RefractionTexture = null;
            }
            foreach (KeyValuePair<Camera, Camera> pair in this.m_ReflectionCameras)
            {
                DestroyImmediate(pair.Value.gameObject);
            }
            this.m_ReflectionCameras.Clear();
            foreach (KeyValuePair<Camera, Camera> pair2 in this.m_RefractionCameras)
            {
                DestroyImmediate(pair2.Value.gameObject);
            }
            this.m_RefractionCameras.Clear();
        }

        public void OnWillRenderObject()
        {
            if (base.enabled && (base.GetComponent<Renderer>() && (base.GetComponent<Renderer>().sharedMaterial && base.GetComponent<Renderer>().enabled)))
            {
                Camera current = Camera.current;
                if (current && !s_InsideWater)
                {
                    Camera camera2;
                    Camera camera3;
                    s_InsideWater = true;
                    this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
                    WaterMode waterMode = this.GetWaterMode();
                    this.CreateWaterObjects(current, out camera2, out camera3);
                    Vector3 position = base.transform.position;
                    Vector3 up = base.transform.up;
                    int pixelLightCount = QualitySettings.pixelLightCount;
                    if (this.disablePixelLights)
                    {
                        QualitySettings.pixelLightCount = 0;
                    }
                    this.UpdateCameraModes(current, camera2);
                    this.UpdateCameraModes(current, camera3);
                    if (waterMode >= WaterMode.Reflective)
                    {
                        float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
                        Vector4 plane = new Vector4(up.x, up.y, up.z, w);
                        Matrix4x4 zero = Matrix4x4.zero;
                        CalculateReflectionMatrix(ref zero, plane);
                        Vector3 v = current.transform.position;
                        Vector3 vector5 = zero.MultiplyPoint(v);
                        camera2.worldToCameraMatrix = current.worldToCameraMatrix * zero;
                        camera2.projectionMatrix = current.CalculateObliqueMatrix(this.CameraSpacePlane(camera2, position, up, 1f));
                        camera2.cullingMask = -17 & this.reflectLayers.value;
                        camera2.targetTexture = this.m_ReflectionTexture;
                        bool invertCulling = GL.invertCulling;
                        GL.invertCulling = !invertCulling;
                        camera2.transform.position = vector5;
                        Vector3 eulerAngles = current.transform.eulerAngles;
                        camera2.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
                        camera2.Render();
                        camera2.transform.position = v;
                        GL.invertCulling = invertCulling;
                        base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
                    }
                    if (waterMode >= WaterMode.Refractive)
                    {
                        camera3.worldToCameraMatrix = current.worldToCameraMatrix;
                        camera3.projectionMatrix = current.CalculateObliqueMatrix(this.CameraSpacePlane(camera3, position, up, -1f));
                        camera3.cullingMask = -17 & this.refractLayers.value;
                        camera3.targetTexture = this.m_RefractionTexture;
                        camera3.transform.position = current.transform.position;
                        camera3.transform.rotation = current.transform.rotation;
                        camera3.Render();
                        base.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", this.m_RefractionTexture);
                    }
                    if (this.disablePixelLights)
                    {
                        QualitySettings.pixelLightCount = pixelLightCount;
                    }
                    if (waterMode == WaterMode.Simple)
                    {
                        Shader.EnableKeyword("WATER_SIMPLE");
                        Shader.DisableKeyword("WATER_REFLECTIVE");
                        Shader.DisableKeyword("WATER_REFRACTIVE");
                    }
                    else if (waterMode == WaterMode.Reflective)
                    {
                        Shader.DisableKeyword("WATER_SIMPLE");
                        Shader.EnableKeyword("WATER_REFLECTIVE");
                        Shader.DisableKeyword("WATER_REFRACTIVE");
                    }
                    else if (waterMode == WaterMode.Refractive)
                    {
                        Shader.DisableKeyword("WATER_SIMPLE");
                        Shader.DisableKeyword("WATER_REFLECTIVE");
                        Shader.EnableKeyword("WATER_REFRACTIVE");
                    }
                    s_InsideWater = false;
                }
            }
        }

        private void Update()
        {
            if (base.GetComponent<Renderer>())
            {
                Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
                if (sharedMaterial)
                {
                    Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
                    float @float = sharedMaterial.GetFloat("_WaveScale");
                    Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
                    double num2 = ((double) Time.timeSinceLevelLoad) / 20.0;
                    Vector4 vector3 = new Vector4((float) Math.IEEERemainder((vector.x * vector2.x) * num2, 1.0), (float) Math.IEEERemainder((vector.y * vector2.y) * num2, 1.0), (float) Math.IEEERemainder((vector.z * vector2.z) * num2, 1.0), (float) Math.IEEERemainder((vector.w * vector2.w) * num2, 1.0));
                    sharedMaterial.SetVector("_WaveOffset", vector3);
                    sharedMaterial.SetVector("_WaveScale4", vector2);
                }
            }
        }

        private void UpdateCameraModes(Camera src, Camera dest)
        {
            if (dest != null)
            {
                dest.clearFlags = src.clearFlags;
                dest.backgroundColor = src.backgroundColor;
                if (src.clearFlags == CameraClearFlags.Skybox)
                {
                    Skybox component = src.GetComponent<Skybox>();
                    Skybox skybox2 = dest.GetComponent<Skybox>();
                    if (!component || !component.material)
                    {
                        skybox2.enabled = false;
                    }
                    else
                    {
                        skybox2.enabled = true;
                        skybox2.material = component.material;
                    }
                }
                dest.farClipPlane = src.farClipPlane;
                dest.nearClipPlane = src.nearClipPlane;
                dest.orthographic = src.orthographic;
                dest.fieldOfView = src.fieldOfView;
                dest.aspect = src.aspect;
                dest.orthographicSize = src.orthographicSize;
            }
        }

        public enum WaterMode
        {
            Simple,
            Reflective,
            Refractive
        }
    }
}

