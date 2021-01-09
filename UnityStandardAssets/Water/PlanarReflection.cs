namespace UnityStandardAssets.Water
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(WaterBase))]
    public class PlanarReflection : MonoBehaviour
    {
        public LayerMask reflectionMask;
        public bool reflectSkybox;
        public Color clearColor = Color.grey;
        public string reflectionSampler = "_ReflectionTex";
        public float clipPlaneOffset = 0.07f;
        private Vector3 m_Oldpos;
        private Camera m_ReflectionCamera;
        private Material m_SharedMaterial;
        private Dictionary<Camera, bool> m_HelperCameras;

        private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
        {
            Vector4 b = (Vector4) (projection.inverse * new Vector4(Sgn(clipPlane.x), Sgn(clipPlane.y), 1f, 1f));
            Vector4 vector2 = clipPlane * (2f / Vector4.Dot(clipPlane, b));
            projection[2] = vector2.x - projection[3];
            projection[6] = vector2.y - projection[7];
            projection[10] = vector2.z - projection[11];
            projection[14] = vector2.w - projection[15];
            return projection;
        }

        private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
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
            return reflectionMat;
        }

        private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
        {
            Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
            Vector3 lhs = worldToCameraMatrix.MultiplyPoint(pos + (normal * this.clipPlaneOffset));
            Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
        }

        private Camera CreateReflectionCameraFor(Camera cam)
        {
            string name = base.gameObject.name + "Reflection" + cam.name;
            GameObject obj2 = GameObject.Find(name);
            if (!obj2)
            {
                Type[] components = new Type[] { typeof(Camera) };
                obj2 = new GameObject(name, components);
            }
            if (!obj2.GetComponent(typeof(Camera)))
            {
                obj2.AddComponent(typeof(Camera));
            }
            Camera component = obj2.GetComponent<Camera>();
            component.backgroundColor = this.clearColor;
            component.clearFlags = !this.reflectSkybox ? CameraClearFlags.Color : CameraClearFlags.Skybox;
            this.SetStandardCameraParameter(component, this.reflectionMask);
            if (!component.targetTexture)
            {
                component.targetTexture = this.CreateTextureFor(cam);
            }
            return component;
        }

        private RenderTexture CreateTextureFor(Camera cam) => 
            new RenderTexture(Mathf.FloorToInt(cam.pixelWidth * 0.5f), Mathf.FloorToInt(cam.pixelHeight * 0.5f), 0x18) { hideFlags = HideFlags.DontSave };

        public void LateUpdate()
        {
            if (this.m_HelperCameras != null)
            {
                this.m_HelperCameras.Clear();
            }
        }

        public void OnDisable()
        {
            Shader.EnableKeyword("WATER_SIMPLE");
            Shader.DisableKeyword("WATER_REFLECTIVE");
        }

        public void OnEnable()
        {
            Shader.EnableKeyword("WATER_REFLECTIVE");
            Shader.DisableKeyword("WATER_SIMPLE");
        }

        public void RenderHelpCameras(Camera currentCam)
        {
            this.m_HelperCameras ??= new Dictionary<Camera, bool>();
            if (!this.m_HelperCameras.ContainsKey(currentCam))
            {
                this.m_HelperCameras.Add(currentCam, false);
            }
            if (!this.m_HelperCameras[currentCam])
            {
                if (!this.m_ReflectionCamera)
                {
                    this.m_ReflectionCamera = this.CreateReflectionCameraFor(currentCam);
                }
                this.RenderReflectionFor(currentCam, this.m_ReflectionCamera);
                this.m_HelperCameras[currentCam] = true;
            }
        }

        private void RenderReflectionFor(Camera cam, Camera reflectCamera)
        {
            if (reflectCamera && (!this.m_SharedMaterial || this.m_SharedMaterial.HasProperty(this.reflectionSampler)))
            {
                reflectCamera.cullingMask = ((int) this.reflectionMask) & ~(1 << (LayerMask.NameToLayer("Water") & 0x1f));
                this.SaneCameraSettings(reflectCamera);
                reflectCamera.backgroundColor = this.clearColor;
                reflectCamera.clearFlags = !this.reflectSkybox ? CameraClearFlags.Color : CameraClearFlags.Skybox;
                if (this.reflectSkybox && cam.gameObject.GetComponent(typeof(Skybox)))
                {
                    Skybox component = (Skybox) reflectCamera.gameObject.GetComponent(typeof(Skybox));
                    if (!component)
                    {
                        component = (Skybox) reflectCamera.gameObject.AddComponent(typeof(Skybox));
                    }
                    component.material = ((Skybox) cam.GetComponent(typeof(Skybox))).material;
                }
                GL.invertCulling = true;
                Transform transform = base.transform;
                Vector3 eulerAngles = cam.transform.eulerAngles;
                reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
                reflectCamera.transform.position = cam.transform.position;
                Vector3 position = transform.transform.position;
                position.y = transform.position.y;
                Vector3 up = transform.transform.up;
                float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
                Vector4 plane = new Vector4(up.x, up.y, up.z, w);
                Matrix4x4 matrixx = CalculateReflectionMatrix(Matrix4x4.zero, plane);
                this.m_Oldpos = cam.transform.position;
                Vector3 vector6 = matrixx.MultiplyPoint(this.m_Oldpos);
                reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrixx;
                Matrix4x4 matrixx2 = CalculateObliqueMatrix(cam.projectionMatrix, this.CameraSpacePlane(reflectCamera, position, up, 1f));
                reflectCamera.projectionMatrix = matrixx2;
                reflectCamera.transform.position = vector6;
                Vector3 vector8 = cam.transform.eulerAngles;
                reflectCamera.transform.eulerAngles = new Vector3(-vector8.x, vector8.y, vector8.z);
                reflectCamera.Render();
                GL.invertCulling = false;
            }
        }

        private void SaneCameraSettings(Camera helperCam)
        {
            helperCam.depthTextureMode = DepthTextureMode.None;
            helperCam.backgroundColor = Color.black;
            helperCam.clearFlags = CameraClearFlags.Color;
            helperCam.renderingPath = RenderingPath.Forward;
        }

        private void SetStandardCameraParameter(Camera cam, LayerMask mask)
        {
            cam.cullingMask = ((int) mask) & ~(1 << (LayerMask.NameToLayer("Water") & 0x1f));
            cam.backgroundColor = Color.black;
            cam.enabled = false;
        }

        private static float Sgn(float a) => 
            (a <= 0f) ? ((a >= 0f) ? 0f : -1f) : 1f;

        public void Start()
        {
            this.m_SharedMaterial = ((WaterBase) base.gameObject.GetComponent(typeof(WaterBase))).sharedMaterial;
        }

        public void WaterTileBeingRendered(Transform tr, Camera currentCam)
        {
            this.RenderHelpCameras(currentCam);
            if (this.m_ReflectionCamera && this.m_SharedMaterial)
            {
                this.m_SharedMaterial.SetTexture(this.reflectionSampler, this.m_ReflectionCamera.targetTexture);
            }
        }
    }
}

