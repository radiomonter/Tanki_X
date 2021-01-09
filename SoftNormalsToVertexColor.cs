using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SoftNormalsToVertexColor : MonoBehaviour
{
    public Method method = Method.AngularDeviation;
    public bool generateOnAwake;
    public bool generateNow;

    private void Awake()
    {
        if (this.generateOnAwake)
        {
            this.TryGenerate();
        }
    }

    private void Generate(Mesh m)
    {
        Vector3[] normals = m.normals;
        Vector3[] vertices = m.vertices;
        Color[] colorArray = new Color[normals.Length];
        List<List<int>> list = new List<List<int>>();
        for (int i = 0; i < vertices.Length; i++)
        {
            bool flag = false;
            foreach (List<int> list2 in list)
            {
                if (vertices[list2[0]] == vertices[i])
                {
                    list2.Add(i);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                List<int> item = new List<int> {
                    i
                };
                list.Add(item);
            }
        }
        foreach (List<int> list4 in list)
        {
            Vector3 zero = Vector3.zero;
            foreach (int num2 in list4)
            {
                zero += normals[num2];
            }
            zero.Normalize();
            if (this.method == Method.AngularDeviation)
            {
                float num3 = 0f;
                foreach (int num4 in list4)
                {
                    num3 += Vector3.Dot(normals[num4], zero);
                }
                zero *= 0.5f / Mathf.Sin(((180f - (Mathf.Acos(num3 / ((float) list4.Count)) * 57.29578f)) - 90f) * 0.01745329f);
            }
            foreach (int num8 in list4)
            {
                colorArray[num8] = new Color(zero.x, zero.y, zero.z);
            }
        }
        m.colors = colorArray;
    }

    private void OnDrawGizmos()
    {
        if (this.generateNow)
        {
            this.generateNow = false;
            this.TryGenerate();
        }
    }

    private void TryGenerate()
    {
        MeshFilter component = base.GetComponent<MeshFilter>();
        if (component == null)
        {
            Debug.LogError("MeshFilter missing on the vertex color generator", base.gameObject);
        }
        else if (component.sharedMesh == null)
        {
            Debug.LogError("Assign a mesh to the MeshFilter before generating vertex colors", base.gameObject);
        }
        else
        {
            this.Generate(component.sharedMesh);
            Debug.Log("Vertex colors generated", base.gameObject);
        }
    }

    public enum Method
    {
        Simple,
        AngularDeviation
    }
}

