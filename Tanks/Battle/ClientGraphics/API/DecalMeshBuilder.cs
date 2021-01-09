namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class DecalMeshBuilder
    {
        public MeshBuilder collectedPolygonsMeshBuilder = new MeshBuilder();
        public MeshBuilder meshBuilder = new MeshBuilder();
        private MeshBuffersCache meshBuffersCache = new MeshBuffersCache();
        private DecalPolygonCollector decalPolygonCollector;
        private DecalMeshClipper decalMeshClipper;
        private DecalUVProjector decalUVProjector;

        public DecalMeshBuilder()
        {
            this.decalPolygonCollector = new DecalPolygonCollector(this.meshBuffersCache);
            this.decalMeshClipper = new DecalMeshClipper();
            this.decalUVProjector = new DecalUVProjector();
        }

        public bool Build(DecalProjection decalProjection, ref Mesh mesh)
        {
            this.Clean();
            if (!this.CompleteProjectionByRaycast(decalProjection))
            {
                return false;
            }
            if (!this.CollectPolygons(decalProjection))
            {
                return false;
            }
            this.BuilldDecalFromCollectedPolygons(decalProjection);
            this.GetResultToMesh(ref mesh);
            return true;
        }

        public bool BuilldDecalFromCollectedPolygons(DecalProjection decalProjection)
        {
            this.CleanResult();
            this.decalMeshClipper.Clip(decalProjection, this.collectedPolygonsMeshBuilder, this.meshBuilder);
            this.decalUVProjector.Project(this.meshBuilder, decalProjection);
            return true;
        }

        public void Clean()
        {
            this.CleanCollectedPolygons();
            this.CleanResult();
        }

        public void CleanCollectedPolygons()
        {
            this.collectedPolygonsMeshBuilder.Clear();
        }

        public void CleanResult()
        {
            this.meshBuilder.Clear();
        }

        public bool CollectPolygons(DecalProjection decalProjection) => 
            this.decalPolygonCollector.Collect(decalProjection, this.collectedPolygonsMeshBuilder);

        public bool CompleteProjectionByRaycast(DecalProjection decalProjection)
        {
            RaycastHit hit;
            if (!Physics.Raycast(decalProjection.Ray.origin, decalProjection.Ray.direction, out hit, decalProjection.Distantion, LayerMasks.VISUAL_STATIC))
            {
                return false;
            }
            decalProjection.ProjectionHit = hit;
            return (hit.normal.Equals(Vector3.zero) || true);
        }

        public bool GetResultToMesh(ref Mesh mesh)
        {
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.MarkDynamic();
            }
            return this.meshBuilder.BuildToMesh(mesh, true);
        }

        public void WarmupMeshCaches(GameObject root)
        {
            foreach (MeshCollider collider in root.GetComponentsInChildren<MeshCollider>())
            {
                Mesh sharedMesh = collider.sharedMesh;
                if ((sharedMesh != null) && (collider.gameObject.GetComponent<ParentRendererBehaviour>() != null))
                {
                    float[] numArray;
                    Vector3[] vectorArray;
                    this.meshBuffersCache.GetTriangles(sharedMesh);
                    this.meshBuffersCache.GetVertices(sharedMesh);
                    this.meshBuffersCache.GetNormals(sharedMesh);
                    this.meshBuffersCache.GetTriangleRadius(sharedMesh, out numArray, out vectorArray);
                }
            }
        }
    }
}

