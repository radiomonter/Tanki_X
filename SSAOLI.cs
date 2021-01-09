using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera)), RequireComponent(typeof(BlurOptimized))]
public class SSAOLI : MonoBehaviour
{
    public float radius = 2f;
    public float maxDistance = 2f;
    public float intensity = 2f;
    private List<Vector4> samples;
    private Texture2D rotationMap;
    private float uvRadiusDepth1;
    public Shader ssaoShader;
    private Material ssaoMaterial;
    private BlurOptimized blur;

    private void Awake()
    {
        this.ssaoMaterial = new Material(this.ssaoShader);
        this.blur = base.GetComponent<BlurOptimized>();
        this.blur.enabled = false;
        this.uvRadiusDepth1 = this.CalculateUVRadiusDepth1();
        this.samples = this.CalculateSamples();
        this.rotationMap = this.CreateRotationMap();
    }

    private float CalculateLengthInSphere(float x, float y) => 
        2f * Mathf.Sqrt(1f - ((x * x) + (y * y)));

    private List<Vector4> CalculateSamples()
    {
        List<Vector4> samples = this.CreateSamples();
        this.CalculateSampleVolumes(samples);
        this.RemovePairSamples(samples);
        return samples;
    }

    private unsafe void CalculateSampleVolumes(List<Vector4> samples)
    {
        float num = 0f;
        int i = 0;
        while (i < 0x100)
        {
            int num3 = 0;
            while (true)
            {
                if (num3 >= 0x100)
                {
                    i++;
                    break;
                }
                if (IsPointInsideCircle(i, num3, 0x80))
                {
                    int num4 = FindClosestSample(i, num3, 0x80, samples);
                    Vector4 vector = samples[num4];
                    float num5 = this.CalculateLengthInSphere(((float) (0x80 - i)) / 128f, ((float) (0x80 - num3)) / 128f);
                    Vector4* vectorPtr1 = &vector;
                    vectorPtr1->w += num5;
                    samples[num4] = vector;
                    num += num5;
                }
                num3++;
            }
        }
        for (int j = 0; j < samples.Count; j++)
        {
            Vector4 vector2 = samples[j];
            Vector4* vectorPtr2 = &vector2;
            vectorPtr2->w /= num;
            samples[j] = vector2;
        }
    }

    private float CalculateUVRadiusDepth1()
    {
        Camera component = base.GetComponent<Camera>();
        return (component.WorldToViewportPoint(component.transform.localToWorldMatrix.MultiplyPoint(new Vector3(this.radius, 0f, 1f))).x - 0.5f);
    }

    private Texture2D CreateRotationMap()
    {
        Texture2D textured = new Texture2D(4, 4, TextureFormat.RGBA32, false) {
            wrapMode = TextureWrapMode.Repeat
        };
        float f = 0f;
        int x = 0;
        while (x < 4)
        {
            int y = 0;
            while (true)
            {
                if (y >= 4)
                {
                    x++;
                    break;
                }
                textured.SetPixel(x, y, new Color(Mathf.Sin(f), Mathf.Cos(f), 0f));
                f += 0.1963495f;
                y++;
            }
        }
        textured.Apply();
        return textured;
    }

    private List<Vector4> CreateSamples()
    {
        List<Vector4> list = new List<Vector4>();
        Vector4 item = new Vector4 {
            x = 0f,
            y = 0f,
            z = this.CalculateLengthInSphere(0f, 0f)
        };
        list.Add(item);
        float num = 1.047198f;
        float num2 = 0f;
        for (int i = 1; i <= 3; i++)
        {
            float num4 = ((float) i) / 3.5f;
            Vector4 vector3 = new Vector4 {
                x = -num4 * Mathf.Sin(-num2),
                y = num4 * Mathf.Cos(-num2)
            };
            vector3.z = this.CalculateLengthInSphere(vector3.x, vector3.y);
            item = new Vector4 {
                x = -vector3.x,
                y = -vector3.y,
                z = vector3.z
            };
            Vector4 vector4 = item;
            list.Add(vector3);
            list.Add(vector4);
            num2 += num;
        }
        return list;
    }

    private static int FindClosestSample(int x, int y, int radius, List<Vector4> samples)
    {
        float maxValue = float.MaxValue;
        int num2 = -1;
        for (int i = 0; i < samples.Count; i++)
        {
            Vector4 vector = samples[i];
            float num4 = (vector.x * radius) + radius;
            Vector4 vector2 = samples[i];
            float num5 = (vector2.y * radius) + radius;
            float num6 = ((x - num4) * (x - num4)) + ((y - num5) * (y - num5));
            if (num6 < maxValue)
            {
                maxValue = num6;
                num2 = i;
            }
        }
        return num2;
    }

    private static bool IsPointInsideCircle(int i, int j, int radius)
    {
        int num = radius - i;
        int num2 = radius - j;
        return (((num * num) + (num2 * num2)) < (radius * radius));
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        this.ssaoMaterial.SetTexture("_RotationMap", this.rotationMap);
        this.ssaoMaterial.SetFloat("centerVolume", this.samples[0].w);
        this.ssaoMaterial.SetFloat("uvRadiusDepth1", this.uvRadiusDepth1);
        this.ssaoMaterial.SetFloat("radius", this.radius);
        this.ssaoMaterial.SetFloat("maxDistance", this.maxDistance);
        this.ssaoMaterial.SetFloat("intensity", this.intensity);
        for (int i = 1; i < this.samples.Count; i++)
        {
            this.ssaoMaterial.SetVector("sample" + i, this.samples[i]);
        }
        RenderTexture dest = RenderTexture.GetTemporary(source.width, source.height, 0);
        Graphics.Blit(source, dest, this.ssaoMaterial, 0);
        RenderTexture texture2 = RenderTexture.GetTemporary(source.width, source.height, 0);
        this.blur.OnRenderImage(dest, texture2);
        RenderTexture.ReleaseTemporary(dest);
        this.ssaoMaterial.SetTexture("_SSAO", texture2);
        Graphics.Blit(source, destination, this.ssaoMaterial, 1);
        RenderTexture.ReleaseTemporary(texture2);
    }

    private void RemovePairSamples(List<Vector4> samples)
    {
        for (int i = samples.Count - 1; i > 1; i -= 2)
        {
            samples.RemoveAt(i);
        }
    }
}

