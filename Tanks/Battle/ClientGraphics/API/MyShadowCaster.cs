namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MyShadowCaster : MonoBehaviour
    {
        private Transform[] casters;
        private List<Renderer> renderers;
        private bool hasBounds;

        public void Awake()
        {
            this.Casters = new Transform[] { base.transform };
        }

        private void CollectRendereres(Transform element, List<Renderer> renderers)
        {
            bool includeInactive = true;
            Renderer[] componentsInChildren = element.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive);
            Renderer[] collection = element.GetComponentsInChildren<MeshRenderer>(includeInactive);
            renderers.AddRange(componentsInChildren);
            renderers.AddRange(collection);
        }

        public Transform[] Casters
        {
            get => 
                this.casters;
            set
            {
                this.casters = value;
                this.renderers = new List<Renderer>();
                foreach (Transform transform in this.casters)
                {
                    this.CollectRendereres(transform, this.renderers);
                }
                this.hasBounds = this.renderers.Count > 0;
            }
        }

        public List<Renderer> Renderers =>
            this.renderers;

        public bool HasBounds =>
            this.hasBounds;

        public Bounds BoundsInWorldSpace
        {
            get
            {
                Bounds bounds = this.renderers[0].bounds;
                for (int i = 1; i < this.renderers.Count; i++)
                {
                    bounds.Encapsulate(this.renderers[i].bounds);
                }
                return bounds;
            }
        }
    }
}

