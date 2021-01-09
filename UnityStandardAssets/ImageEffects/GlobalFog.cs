namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Rendering/Global Fog")]
    internal class GlobalFog : PostEffectsBase
    {
        [Tooltip("Apply distance-based fog?")]
        public bool distanceFog = true;
        [Tooltip("Distance fog is based on radial distance from camera when checked")]
        public bool useRadialDistance;
        [Tooltip("Apply height-based fog?")]
        public bool heightFog = true;
        [Tooltip("Fog top Y coordinate")]
        public float height = 1f;
        [Range(0.001f, 10f)]
        public float heightDensity = 2f;
        [Tooltip("Push fog away from the camera by this amount")]
        public float startDistance;
        public Shader fogShader;
        private Material fogMaterial;

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.fogMaterial = base.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
        {
            RenderTexture.active = dest;
            fxMaterial.SetTexture("_MainTex", source);
            GL.PushMatrix();
            GL.LoadOrtho();
            fxMaterial.SetPass(passNr);
            GL.Begin(7);
            GL.MultiTexCoord2(0, 0f, 0f);
            GL.Vertex3(0f, 0f, 3f);
            GL.MultiTexCoord2(0, 1f, 0f);
            GL.Vertex3(1f, 0f, 2f);
            GL.MultiTexCoord2(0, 1f, 1f);
            GL.Vertex3(1f, 1f, 1f);
            GL.MultiTexCoord2(0, 0f, 1f);
            GL.Vertex3(0f, 1f, 0f);
            GL.End();
            GL.PopMatrix();
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources() || (!this.distanceFog && !this.heightFog))
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                Vector4 vector8;
                Camera component = base.GetComponent<Camera>();
                Transform transform = component.transform;
                float nearClipPlane = component.nearClipPlane;
                float farClipPlane = component.farClipPlane;
                float aspect = component.aspect;
                Matrix4x4 identity = Matrix4x4.identity;
                float num5 = component.fieldOfView * 0.5f;
                Vector3 vector = ((transform.right * nearClipPlane) * Mathf.Tan(num5 * 0.01745329f)) * aspect;
                Vector3 vector2 = (transform.up * nearClipPlane) * Mathf.Tan(num5 * 0.01745329f);
                Vector3 vector3 = ((transform.forward * nearClipPlane) - vector) + vector2;
                float num6 = (vector3.magnitude * farClipPlane) / nearClipPlane;
                vector3.Normalize();
                Vector3 vector4 = ((transform.forward * nearClipPlane) + vector) + vector2;
                vector4.Normalize();
                Vector3 vector5 = ((transform.forward * nearClipPlane) + vector) - vector2;
                vector5.Normalize();
                Vector3 vector6 = ((transform.forward * nearClipPlane) - vector) - vector2;
                vector6.Normalize();
                identity.SetRow(0, vector3 * num6);
                identity.SetRow(1, vector4 * num6);
                identity.SetRow(2, vector5 * num6);
                identity.SetRow(3, vector6 * num6);
                Vector3 position = transform.position;
                float y = position.y - this.height;
                this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
                this.fogMaterial.SetVector("_CameraWS", position);
                this.fogMaterial.SetVector("_HeightParams", new Vector4(this.height, y, (y > 0f) ? 0f : 1f, this.heightDensity * 0.5f));
                this.fogMaterial.SetVector("_DistanceParams", new Vector4(-Mathf.Max(this.startDistance, 0f), 0f, 0f, 0f));
                FogMode fogMode = RenderSettings.fogMode;
                float fogDensity = RenderSettings.fogDensity;
                float fogStartDistance = RenderSettings.fogStartDistance;
                float fogEndDistance = RenderSettings.fogEndDistance;
                bool flag = fogMode == FogMode.Linear;
                float f = !flag ? 0f : (fogEndDistance - fogStartDistance);
                float num13 = (Mathf.Abs(f) <= 0.0001f) ? 0f : (1f / f);
                vector8.x = fogDensity * 1.201122f;
                vector8.y = fogDensity * 1.442695f;
                vector8.z = !flag ? 0f : -num13;
                vector8.w = !flag ? 0f : (fogEndDistance * num13);
                this.fogMaterial.SetVector("_SceneFogParams", vector8);
                this.fogMaterial.SetVector("_SceneFogMode", new Vector4((float) fogMode, !this.useRadialDistance ? ((float) 0) : ((float) 1), 0f, 0f));
                int passNr = 0;
                passNr = (!this.distanceFog || !this.heightFog) ? (!this.distanceFog ? 2 : 1) : 0;
                CustomGraphicsBlit(source, destination, this.fogMaterial, passNr);
            }
        }
    }
}

