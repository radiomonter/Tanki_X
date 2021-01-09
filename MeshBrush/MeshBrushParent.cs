namespace MeshBrush
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class MeshBrushParent : MonoBehaviour
    {
        private Transform[] meshes;
        private Component[] meshFilters;
        private Matrix4x4 myTransform;
        private Hashtable materialToMesh;
        private MeshFilter filter;
        private Renderer curRenderer;
        private Material[] materials;
        private CombineUtility.MeshInstance instance;
        private CombineUtility.MeshInstance[] instances;
        private ArrayList objects;
        private ArrayList elements;

        private void Start()
        {
            Destroy(this);
        }
    }
}

