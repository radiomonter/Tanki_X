namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public abstract class GenericDecals<D, P, DM> : GenericDecalsBase where D: GenericDecals<D, P, DM> where P: GenericDecalProjector<D, P, DM> where DM: GenericDecalsMesh<D, P, DM>
    {
        protected GenericDecals()
        {
        }

        public virtual void UpdateDecalsMeshes(DM a_DecalsMesh)
        {
            if (a_DecalsMesh == null)
            {
                throw new NullReferenceException("The passed decals mesh is null.");
            }
            if (a_DecalsMesh.Decals != this)
            {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }
            a_DecalsMesh.PreservedVertexColorArrays.Clear();
            a_DecalsMesh.PreservedProjectedUVArrays.Clear();
            a_DecalsMesh.PreservedProjectedUV2Arrays.Clear();
        }

        public virtual void UpdateProjectedUV2s(DM a_DecalsMesh)
        {
            if (Edition.IsDecalSystemFree)
            {
                throw new InvalidOperationException("This function is only supported in Decal System Pro.");
            }
            if (a_DecalsMesh == null)
            {
                throw new NullReferenceException("The passed decals mesh is null.");
            }
            if (a_DecalsMesh.Decals != this)
            {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }
            if ((base.CurrentUV2Mode != UV2Mode.Project) && (base.CurrentUV2Mode != UV2Mode.ProjectWrapped))
            {
                throw new InvalidOperationException("The current uv2 mode is not projecting.");
            }
            if (!a_DecalsMesh.PreserveProjectedUV2Arrays)
            {
                throw new InvalidOperationException("Projected UV2s can only be updated if the decals mesh preserves them.");
            }
            int length = 0;
            foreach (Vector2[] vectorArray in a_DecalsMesh.PreservedProjectedUV2Arrays)
            {
                length = vectorArray.Length;
            }
            if (length != a_DecalsMesh.UV2s.Count)
            {
                throw new InvalidOperationException("The preserved UV2 count doesn't match the one from the decals mesh. Avoid changes on the decals mesh which affect the number of vertices, after it was used to update the decals instance as preserving arrays are used.");
            }
        }

        public virtual void UpdateProjectedUVs(DM a_DecalsMesh)
        {
            if (Edition.IsDecalSystemFree)
            {
                throw new InvalidOperationException("This function is only supported in Decal System Pro.");
            }
            if (a_DecalsMesh == null)
            {
                throw new NullReferenceException("The passed decals mesh is null.");
            }
            if (a_DecalsMesh.Decals != this)
            {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }
            if ((base.CurrentUVMode != UVMode.Project) && (base.CurrentUVMode != UVMode.ProjectWrapped))
            {
                throw new InvalidOperationException("The current uv mode is not projecting.");
            }
            if (!a_DecalsMesh.PreserveProjectedUVArrays)
            {
                throw new InvalidOperationException("Projected UVs can only be updated if the decals mesh preserves them.");
            }
            int length = 0;
            foreach (Vector2[] vectorArray in a_DecalsMesh.PreservedProjectedUVArrays)
            {
                length = vectorArray.Length;
            }
            if (length != a_DecalsMesh.UVs.Count)
            {
                throw new InvalidOperationException("The preserved UV count doesn't match the one from the decals mesh. Avoid changes on the decals mesh which affect the number of vertices, after it was used to update the decals instance as preserving arrays are used.");
            }
        }

        public virtual void UpdateVertexColors(DM a_DecalsMesh)
        {
            if (Edition.IsDecalSystemFree)
            {
                throw new InvalidOperationException("This function is only supported in Decal System Pro.");
            }
            if (a_DecalsMesh == null)
            {
                throw new NullReferenceException("The passed decals mesh is null.");
            }
            if (a_DecalsMesh.Decals != this)
            {
                throw new InvalidOperationException("The decals mesh is not linked to this decals instance.");
            }
            if (!base.UseVertexColors)
            {
                throw new InvalidOperationException("Vertex colors are not used.");
            }
            if (!a_DecalsMesh.PreserveVertexColorArrays)
            {
                throw new InvalidOperationException("Vertex colors can only be updated if the decals mesh preserves them.");
            }
            int length = 0;
            foreach (Color[] colorArray in a_DecalsMesh.PreservedVertexColorArrays)
            {
                length = colorArray.Length;
            }
            if (length != a_DecalsMesh.VertexColors.Count)
            {
                throw new InvalidOperationException("The preserved vertex color count doesn't match the one from the decals mesh. Avoid changes on the decals mesh which affect the number of vertices, after it was used to update the decals instance as preserving arrays are used.");
            }
        }
    }
}

