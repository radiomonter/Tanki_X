namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientGraphics;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class DecalPolygonCollector
    {
        private MeshBuffersCache cache;

        public DecalPolygonCollector(MeshBuffersCache meshBuffersCache)
        {
            this.cache = meshBuffersCache;
        }

        public bool Collect(DecalProjection projection, MeshBuilder meshBuilder)
        {
            foreach (Collider collider in this.GetColliders(projection))
            {
                Mesh mesh;
                if (this.GetMeshByCollider(collider, out mesh))
                {
                    Renderer renderer;
                    meshBuilder.ClearWeldHashing();
                    Transform transform = collider.transform;
                    if (this.TryGetRenderer(collider.gameObject, out renderer) && !renderer.gameObject.CompareTag("IgnoreDecals"))
                    {
                        float[] numArray2;
                        Vector3[] vectorArray3;
                        int[] triangles = this.cache.GetTriangles(mesh);
                        Vector3[] vertices = this.cache.GetVertices(mesh);
                        Vector3[] normals = this.cache.GetNormals(mesh);
                        SurfaceType[] surfaceTypes = this.cache.GetSurfaceTypes(mesh, renderer);
                        this.cache.GetTriangleRadius(mesh, out numArray2, out vectorArray3);
                        Vector3 vector = transform.InverseTransformDirection(projection.Ray.direction);
                        Vector3 vector2 = transform.InverseTransformPoint(projection.ProjectionHit.point);
                        for (int i = 0; i < triangles.Length; i += 3)
                        {
                            int index = i / 3;
                            Vector3 vector3 = vectorArray3[index] - vector2;
                            float sqrMagnitude = vector3.sqrMagnitude;
                            float num5 = numArray2[index] + projection.HalfSize;
                            if (sqrMagnitude <= (num5 * num5))
                            {
                                int num6 = triangles[i];
                                if (Vector3.Dot(normals[num6], -vector) >= 0f)
                                {
                                    int num8 = triangles[i + 1];
                                    int num9 = triangles[i + 2];
                                    Vector3 vertex = vertices[num6];
                                    Vector3 vector6 = vertices[num8];
                                    Vector3 vector7 = vertices[num9];
                                    num6 = meshBuilder.AddVertexWeldAndTransform((long) num6, new VertexData(vertex, normals[num6], surfaceTypes[num6]), transform);
                                    meshBuilder.AddTriangle(num6, meshBuilder.AddVertexWeldAndTransform((long) num8, new VertexData(vector6, normals[num8], surfaceTypes[num8]), transform), meshBuilder.AddVertexWeldAndTransform((long) num9, new VertexData(vector7, normals[num9], surfaceTypes[num9]), transform));
                                }
                            }
                        }
                    }
                }
            }
            return (meshBuilder.Triangles.Count > 0);
        }

        private Collider[] GetColliders(DecalProjection projection) => 
            Physics.OverlapSphere(projection.ProjectionHit.point, projection.HalfSize);

        private bool GetMeshByCollider(Collider collider, out Mesh mesh)
        {
            mesh = null;
            MeshCollider collider2 = collider as MeshCollider;
            if (collider2 == null)
            {
                return false;
            }
            mesh = collider2.sharedMesh;
            return (mesh != null);
        }

        private bool TriangleSphereIntersectSphere(Vector3 triangleV0, Vector3 triangleV1, Vector3 triangleV2, Vector3 spherePosition, float radius)
        {
            bool flag = false;
            Vector3 vector = ((triangleV0 + triangleV1) + triangleV2) / 3f;
            float num = (vector - spherePosition).magnitude - radius;
            if (num < 0f)
            {
                flag = true;
            }
            else
            {
                float num2 = num * num;
                flag = (((vector - triangleV0).sqrMagnitude > num2) || ((vector - triangleV1).sqrMagnitude > num2)) || ((vector - triangleV2).sqrMagnitude > num2);
            }
            return flag;
        }

        private bool TryGetRenderer(GameObject gameObject, out Renderer renderer)
        {
            renderer = null;
            ParentRendererBehaviour component = gameObject.GetComponent<ParentRendererBehaviour>();
            if (component != null)
            {
                renderer = component.ParentRenderer;
            }
            return (renderer != null);
        }
    }
}

