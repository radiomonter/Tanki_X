namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera))]
    internal class PostEffectsHelper : MonoBehaviour
    {
        private static void DrawBorder(RenderTexture dest, Material material)
        {
            RenderTexture.active = dest;
            bool flag = true;
            GL.PushMatrix();
            GL.LoadOrtho();
            for (int i = 0; i < material.passCount; i++)
            {
                float num6;
                float num7;
                material.SetPass(i);
                if (flag)
                {
                    num6 = 1f;
                    num7 = 0f;
                }
                else
                {
                    num6 = 0f;
                    num7 = 1f;
                }
                float x = 0f;
                float num2 = 1f / (dest.width * 1f);
                float y = 0f;
                float num4 = 1f;
                GL.Begin(7);
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                x = 1f - (1f / (dest.width * 1f));
                num2 = 1f;
                y = 0f;
                num4 = 1f;
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                x = 0f;
                num2 = 1f;
                y = 0f;
                num4 = 1f / (dest.height * 1f);
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                x = 0f;
                num2 = 1f;
                y = 1f - (1f / (dest.height * 1f));
                num4 = 1f;
                GL.TexCoord2(0f, num6);
                GL.Vertex3(x, y, 0.1f);
                GL.TexCoord2(1f, num6);
                GL.Vertex3(num2, y, 0.1f);
                GL.TexCoord2(1f, num7);
                GL.Vertex3(num2, num4, 0.1f);
                GL.TexCoord2(0f, num7);
                GL.Vertex3(x, num4, 0.1f);
                GL.End();
            }
            GL.PopMatrix();
        }

        private static void DrawLowLevelPlaneAlignedWithCamera(float dist, RenderTexture source, RenderTexture dest, Material material, Camera cameraForProjectionMatrix)
        {
            RenderTexture.active = dest;
            material.SetTexture("_MainTex", source);
            bool flag = true;
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.LoadProjectionMatrix(cameraForProjectionMatrix.projectionMatrix);
            float f = (cameraForProjectionMatrix.fieldOfView * 0.5f) * 0.01745329f;
            float num2 = Mathf.Cos(f) / Mathf.Sin(f);
            float aspect = cameraForProjectionMatrix.aspect;
            float num8 = 1f;
            float x = (aspect / -num2) * (dist * num8);
            float num5 = (aspect / num2) * (dist * num8);
            float y = (1f / -num2) * (dist * num8);
            float num7 = (1f / num2) * (dist * num8);
            float z = -dist;
            for (int i = 0; i < material.passCount; i++)
            {
                float num11;
                float num12;
                material.SetPass(i);
                GL.Begin(7);
                if (flag)
                {
                    num11 = 1f;
                    num12 = 0f;
                }
                else
                {
                    num11 = 0f;
                    num12 = 1f;
                }
                GL.TexCoord2(0f, num11);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(1f, num11);
                GL.Vertex3(num5, y, z);
                GL.TexCoord2(1f, num12);
                GL.Vertex3(num5, num7, z);
                GL.TexCoord2(0f, num12);
                GL.Vertex3(x, num7, z);
                GL.End();
            }
            GL.PopMatrix();
        }

        private static void DrawLowLevelQuad(float x1, float x2, float y1, float y2, RenderTexture source, RenderTexture dest, Material material)
        {
            RenderTexture.active = dest;
            material.SetTexture("_MainTex", source);
            bool flag = true;
            GL.PushMatrix();
            GL.LoadOrtho();
            for (int i = 0; i < material.passCount; i++)
            {
                float num2;
                float num3;
                material.SetPass(i);
                GL.Begin(7);
                if (flag)
                {
                    num2 = 1f;
                    num3 = 0f;
                }
                else
                {
                    num2 = 0f;
                    num3 = 1f;
                }
                GL.TexCoord2(0f, num2);
                GL.Vertex3(x1, y1, 0.1f);
                GL.TexCoord2(1f, num2);
                GL.Vertex3(x2, y1, 0.1f);
                GL.TexCoord2(1f, num3);
                GL.Vertex3(x2, y2, 0.1f);
                GL.TexCoord2(0f, num3);
                GL.Vertex3(x1, y2, 0.1f);
                GL.End();
            }
            GL.PopMatrix();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Debug.Log("OnRenderImage in Helper called ...");
        }
    }
}

