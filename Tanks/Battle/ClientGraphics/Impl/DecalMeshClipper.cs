namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class DecalMeshClipper
    {
        private PolygonClipper2D polygonClipper = new PolygonClipper2D();
        private List<ClipPointData> polygon = new List<ClipPointData>(10);

        private void AddAllPolygonPointsAndTransformToWorld(MeshBuilder builder, ref List<ClipPointData> polygon, Quaternion rotation, Vector3 position)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                ClipPointData data = polygon[i];
                Vector3 vertex = data.vertexData.vertex;
                vertex = ((Vector3) (rotation * vertex)) + position;
                data.vertexData.vertex = vertex;
                data.index = builder.AddVertex(data.vertexData);
                polygon[i] = data;
            }
        }

        private ClipEdge2D[] CalculateClipPlane(float width, float height)
        {
            Vector2 from = new Vector2(-width, -height);
            Vector2 to = new Vector2(-width, height);
            Vector2 vector3 = new Vector2(width, height);
            Vector2 vector4 = new Vector2(width, -height);
            return new ClipEdge2D[] { new ClipEdge2D(from, to), new ClipEdge2D(to, vector3), new ClipEdge2D(vector3, vector4), new ClipEdge2D(vector4, from) };
        }

        public void Clip(DecalProjection projection, MeshBuilder sourceBuilder, MeshBuilder destBuilder)
        {
            List<int> triangles = sourceBuilder.Triangles;
            List<Vector3> vertices = sourceBuilder.Vertices;
            List<Vector3> normals = sourceBuilder.Normals;
            List<SurfaceType> surfaceTypes = sourceBuilder.SurfaceTypes;
            Quaternion rotation = Quaternion.LookRotation(-projection.ProjectionHit.normal, projection.Up);
            Quaternion quaternion2 = Quaternion.Inverse(rotation);
            ClipEdge2D[] clipPlane = this.CalculateClipPlane(projection.HalfSize, projection.HalfSize);
            int num = 0;
            while (num < triangles.Count)
            {
                this.polygon.Clear();
                int num2 = 0;
                while (true)
                {
                    if (num2 >= 3)
                    {
                        if (this.ClipByWidthAndHeight(clipPlane, ref this.polygon) && this.ClipByDepth(clipPlane, projection.HalfSize, ref this.polygon))
                        {
                            this.AddAllPolygonPointsAndTransformToWorld(destBuilder, ref this.polygon, rotation, projection.ProjectionHit.point);
                            this.Triangulate(projection, destBuilder, ref this.polygon);
                        }
                        num += 3;
                        break;
                    }
                    int num3 = triangles[num + num2];
                    Vector3 normal = normals[num3];
                    SurfaceType surfaceType = surfaceTypes[num3];
                    RaycastHit projectionHit = projection.ProjectionHit;
                    Vector3 vertex = (Vector3) (quaternion2 * (vertices[num3] - projectionHit.point));
                    this.polygon.Add(new ClipPointData(new VertexData(vertex, normal, surfaceType)));
                    num2++;
                }
            }
        }

        private bool ClipByDepth(ClipEdge2D[] clipPlane, float depthTest, ref List<ClipPointData> polygon)
        {
            bool flag = false;
            for (int i = 0; i < polygon.Count; i++)
            {
                ClipPointData data = polygon[i];
                Vector3 vertex = data.vertexData.vertex;
                if ((vertex.z > depthTest) || (vertex.z < -depthTest))
                {
                    flag = true;
                }
                polygon[i] = data.ToDepthSpace();
            }
            if (flag)
            {
                polygon = this.polygonClipper.GetIntersectedPolygon(polygon, clipPlane);
            }
            return (polygon.Count > 2);
        }

        private bool ClipByWidthAndHeight(ClipEdge2D[] clipPlane, ref List<ClipPointData> polygon)
        {
            polygon = this.polygonClipper.GetIntersectedPolygon(polygon, clipPlane);
            return (polygon.Count > 2);
        }

        private void Triangulate(DecalProjection projection, MeshBuilder builder, ref List<ClipPointData> polygon)
        {
            List<Vector3> vertices = builder.Vertices;
            int index = polygon[0].index;
            Vector3 vector = vertices[index];
            for (int i = 2; i < polygon.Count; i++)
            {
                ClipPointData data2 = polygon[i - 1];
                int num3 = data2.index;
                ClipPointData data3 = polygon[i];
                int num4 = data3.index;
                Vector3 vector2 = vertices[num3];
                Vector3 vector3 = vertices[num4];
                Ray ray = projection.Ray;
                Vector3 vector4 = Vector3.Cross(vector - vector2, vector - vector3);
                if (Vector3.Dot(-ray.direction, vector4.normalized) > 0f)
                {
                    builder.AddTriangle(index, num3, num4);
                }
                else
                {
                    builder.AddTriangle(index, num4, num3);
                }
            }
        }
    }
}

