namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Terrain
    {
        private readonly List<MeshCollider> meshColliders = new List<MeshCollider>();
        private readonly List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        private readonly UnityEngine.Bounds bounds;

        public Terrain(List<GameObject> terrainObjects)
        {
            foreach (GameObject obj2 in terrainObjects)
            {
                this.CollectMeshParts(obj2);
            }
            this.bounds = CalculateBounds(this.meshColliders);
        }

        public static UnityEngine.Bounds CalculateBounds(List<MeshCollider> meshColliders)
        {
            if (meshColliders.Count == 0)
            {
                return new UnityEngine.Bounds();
            }
            UnityEngine.Bounds bounds = meshColliders[0].bounds;
            for (int i = 1; i < meshColliders.Count; i++)
            {
                bounds.Encapsulate(meshColliders[i].bounds);
            }
            return bounds;
        }

        private void CollectMeshParts(GameObject terrainObject)
        {
            foreach (MeshCollider collider in terrainObject.GetComponentsInChildren<MeshCollider>())
            {
                if ((collider != null) && collider.enabled)
                {
                    this.meshColliders.Add(collider);
                }
            }
            MeshRenderer component = terrainObject.GetComponent<MeshRenderer>();
            if ((component != null) && component.enabled)
            {
                this.meshRenderers.Add(component);
            }
            Transform transform = terrainObject.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                this.CollectMeshParts(transform.GetChild(i).gameObject);
            }
        }

        public List<MeshCollider> MeshColliders =>
            this.meshColliders;

        public List<MeshRenderer> MeshRenderers =>
            this.meshRenderers;

        public UnityEngine.Bounds Bounds =>
            this.bounds;
    }
}

