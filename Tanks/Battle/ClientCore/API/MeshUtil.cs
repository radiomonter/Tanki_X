namespace Tanks.Battle.ClientCore.API
{
    using MIConvexHull;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class MeshUtil
    {
        private static IList<IVertex> ConvertToMIConvexHullVertices(Vector3[] vertices)
        {
            IList<IVertex> list = new List<IVertex>(vertices.Length);
            for (int i = 0; i < vertices.Length; i++)
            {
                Vertex item = new Vertex(vertices[i]);
                list.Add(item);
            }
            return list;
        }

        private static int[] ConvertToUnityTriangles(ConvexHull<IVertex, DefaultConvexFace<IVertex>> convexHull)
        {
            DefaultConvexFace<IVertex>[] faceArray = convexHull.Faces.ToArray<DefaultConvexFace<IVertex>>();
            int[] numArray = new int[faceArray.Length * 3];
            int index = 0;
            while (index < faceArray.Length)
            {
                int num2 = 0;
                while (true)
                {
                    if (num2 >= 3)
                    {
                        index++;
                        break;
                    }
                    numArray[(index * 3) + num2] = ((Vertex) faceArray[index].Vertices[num2]).Index;
                    num2++;
                }
            }
            return numArray;
        }

        private static Vector3[] ConvertToUnityVertices(ConvexHull<IVertex, DefaultConvexFace<IVertex>> convexHull)
        {
            IVertex[] vertexArray = convexHull.Points.ToArray<IVertex>();
            Vector3[] vectorArray = new Vector3[vertexArray.Length];
            for (int i = 0; i < vertexArray.Length; i++)
            {
                Vertex vertex = (Vertex) vertexArray[i];
                vectorArray[i] = vertex.UnityVertex;
                vertex.Index = i;
            }
            return vectorArray;
        }

        public static Mesh CreateConvexMesh(Vector3[] vertices)
        {
            ConvexHull<IVertex, DefaultConvexFace<IVertex>> convexHull = ConvexHull.Create<IVertex>(ConvertToMIConvexHullVertices(vertices), null);
            return new Mesh { 
                vertices = ConvertToUnityVertices(convexHull),
                triangles = ConvertToUnityTriangles(convexHull)
            };
        }
    }
}

