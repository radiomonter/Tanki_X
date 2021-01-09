namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public static class GraphicsBuilderUtils
    {
        public static Renderer GetRenderer(GameObject go)
        {
            Renderer component = go.GetComponent<SkinnedMeshRenderer>();
            if (component == null)
            {
                component = go.GetComponent<MeshRenderer>();
            }
            return component;
        }
    }
}

