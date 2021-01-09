namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    [ExecuteInEditMode]
    public class Environment : MonoBehaviour
    {
        public Cubemap skyCube;
        public Cubemap specCube;
        public float camHDRExposure = 1f;
        public float ambientIntensity = 1f;
        public float reflectionIntensity = 1f;
        private Material _skyboxMaterial;

        private void Awake()
        {
        }

        public SphericalHarmonicsL2 ProjectCubeIntoSH(Cubemap src, int miplevel)
        {
            Vector3[] vectorArray1 = new Vector3[0x12];
            vectorArray1[0] = new Vector3(0f, 0f, -1f);
            vectorArray1[1] = new Vector3(0f, -1f, 0f);
            vectorArray1[2] = new Vector3(-1f, 0f, 0f);
            vectorArray1[3] = new Vector3(0f, 0f, 1f);
            vectorArray1[4] = new Vector3(0f, -1f, 0f);
            vectorArray1[5] = new Vector3(1f, 0f, 0f);
            vectorArray1[6] = new Vector3(1f, 0f, 0f);
            vectorArray1[7] = new Vector3(0f, 0f, 1f);
            vectorArray1[8] = new Vector3(0f, -1f, 0f);
            vectorArray1[9] = new Vector3(1f, 0f, 0f);
            vectorArray1[10] = new Vector3(0f, 0f, -1f);
            vectorArray1[11] = new Vector3(0f, 1f, 0f);
            vectorArray1[12] = new Vector3(1f, 0f, 0f);
            vectorArray1[13] = new Vector3(0f, -1f, 0f);
            vectorArray1[14] = new Vector3(0f, 0f, -1f);
            vectorArray1[15] = new Vector3(-1f, 0f, 0f);
            vectorArray1[0x10] = new Vector3(0f, -1f, 0f);
            vectorArray1[0x11] = new Vector3(0f, 0f, 1f);
            Vector3[] vectorArray = vectorArray1;
            float num = 0f;
            SphericalHarmonicsL2 sl = new SphericalHarmonicsL2();
            sl.Clear();
            int num2 = 0;
            while (num2 < 6)
            {
                Vector3 vector = vectorArray[num2 * 3];
                Vector3 vector2 = -vectorArray[(num2 * 3) + 1];
                Vector3 vector3 = -vectorArray[(num2 * 3) + 2];
                Color[] pixels = src.GetPixels((CubemapFace) num2, miplevel);
                int num3 = src.width >> (miplevel & 0x1f);
                if (num3 < 1)
                {
                    num3 = 1;
                }
                float num4 = -1f + (1f / ((float) num3));
                float num5 = (2f * (1f - (1f / ((float) num3)))) / (num3 - 1f);
                int num6 = 0;
                while (true)
                {
                    if (num6 >= num3)
                    {
                        num2++;
                        break;
                    }
                    float num7 = (num6 * num5) + num4;
                    int num8 = 0;
                    while (true)
                    {
                        if (num8 >= num3)
                        {
                            num6++;
                            break;
                        }
                        Color color = pixels[num8 + (num6 * num3)];
                        float num9 = (num8 * num5) + num4;
                        float f = (1f + (num9 * num9)) + (num7 * num7);
                        float num11 = 4f / (Mathf.Sqrt(f) * f);
                        Vector3 vector4 = (vector3 + (vector * num9)) + (vector2 * num7);
                        vector4.Normalize();
                        Color color2 = (color * color.a) * 8f;
                        sl.AddDirectionalLight(-vector4, (QualitySettings.activeColorSpace != ColorSpace.Linear) ? color2 : color2.linear, num11 * 0.5f);
                        num += num11;
                        num8++;
                    }
                }
            }
            return (sl * (4f / num));
        }

        private Material SkyboxMaterial
        {
            get
            {
                if (this._skyboxMaterial == null)
                {
                    Shader shader = Shader.Find("Skybox/Cubemap");
                    if (!shader)
                    {
                        Debug.LogError("Couldn't find " + shader.name + " shader");
                    }
                    else
                    {
                        this._skyboxMaterial = new Material(shader);
                        this._skyboxMaterial.name = "Skybox";
                    }
                }
                return this._skyboxMaterial;
            }
        }
    }
}

