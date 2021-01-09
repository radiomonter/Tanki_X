namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class GrassPrefabData
    {
        public GameObject grassPrefab;
        public float minScale = 1f;
        public float maxScale = 1f;

        public bool IsValid(out string errorReason)
        {
            if (this.grassPrefab == null)
            {
                errorReason = "GrassPrefab is null";
                return false;
            }
            if (this.grassPrefab.GetComponent<MeshFilter>() == null)
            {
                errorReason = "GrassPrefab hasn't component MeshFilter";
                return false;
            }
            if (this.grassPrefab.GetComponent<MeshFilter>().sharedMesh == null)
            {
                errorReason = "Property sharedMesh of GrassPrefab's component MeshFilter is null";
                return false;
            }
            if (this.grassPrefab.GetComponent<MeshRenderer>() == null)
            {
                errorReason = "GrassPrefab hasn't component MeshRenderer";
                return false;
            }
            if (this.grassPrefab.GetComponent<MeshRenderer>().sharedMaterial == null)
            {
                errorReason = "Property sharedMaterial of GrassPrefab's component MeshRenderer is null";
                return false;
            }
            if (this.minScale.Equals((float) 0f))
            {
                errorReason = "MinScale can't be zero";
                return false;
            }
            if (this.maxScale.Equals((float) 0f))
            {
                errorReason = "MaxScale can't be zero";
                return false;
            }
            if (this.minScale > this.maxScale)
            {
                errorReason = "MinScale can't be more than maxScale";
                return false;
            }
            errorReason = string.Empty;
            return true;
        }

        public override string ToString() => 
            $"[Prefab: {(this.grassPrefab != null) ? this.grassPrefab.name : "null"}, minScale: {this.minScale}, maxScale: {this.maxScale}]";
    }
}

