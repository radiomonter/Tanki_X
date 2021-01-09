namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class PolygonClipper2D
    {
        public List<ClipPointData> inputList = new List<ClipPointData>(10);

        private bool GetIntersect(ClipPointData fromPoint, ClipPointData toPoint, Vector2 clipEdgeFrom, Vector2 clipEdgeTo, out ClipPointData resultPoint)
        {
            Vector2 vector = toPoint.point2D - fromPoint.point2D;
            Vector2 vector2 = clipEdgeTo - clipEdgeFrom;
            float a = (vector.x * vector2.y) - (vector.y * vector2.x);
            if (Mathf.Approximately(a, 0f))
            {
                resultPoint = new ClipPointData();
                return false;
            }
            Vector2 vector3 = clipEdgeFrom - fromPoint.point2D;
            float lerpFactor = ((vector3.x * vector2.y) - (vector3.y * vector2.x)) / a;
            resultPoint = ClipPointData.Lerp(fromPoint, toPoint, lerpFactor);
            return true;
        }

        public List<ClipPointData> GetIntersectedPolygon(List<ClipPointData> polygonPoints, ClipEdge2D[] clipEdges)
        {
            ClipEdge2D edged;
            ClipPointData data;
            int num3;
            ClipPointData data2;
            List<ClipPointData> list = polygonPoints.ToList<ClipPointData>();
            int index = 0;
            goto TR_0017;
        TR_0002:
            data = data2;
            num3++;
            goto TR_000E;
        TR_0005:
            list.Clear();
            return list;
        TR_000E:
            while (true)
            {
                if (num3 < this.inputList.Count)
                {
                    data2 = this.inputList[num3];
                    if (!this.IsInside(edged, data2.point2D))
                    {
                        if (this.IsInside(edged, data.point2D))
                        {
                            ClipPointData data4;
                            if (!this.GetIntersect(data, data2, edged.from, edged.to, out data4))
                            {
                                goto TR_0005;
                            }
                            else
                            {
                                list.Add(data4);
                            }
                        }
                        goto TR_0002;
                    }
                    else
                    {
                        ClipPointData data3;
                        if (this.IsInside(edged, data.point2D))
                        {
                            break;
                        }
                        if (this.GetIntersect(data, data2, edged.from, edged.to, out data3))
                        {
                            list.Add(data3);
                            break;
                        }
                        goto TR_0005;
                    }
                }
                else
                {
                    index++;
                    goto TR_0017;
                }
                break;
            }
            list.Add(data2);
            goto TR_0002;
        TR_0017:
            while (true)
            {
                if (index < clipEdges.Length)
                {
                    edged = clipEdges[index];
                    this.inputList.Clear();
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 < list.Count)
                        {
                            this.inputList.Add(list[num2]);
                            num2++;
                            continue;
                        }
                        list.Clear();
                        if (this.inputList.Count != 0)
                        {
                            data = this.inputList[this.inputList.Count - 1];
                            num3 = 0;
                        }
                        else
                        {
                            return list;
                        }
                        break;
                    }
                }
                else
                {
                    return list;
                }
                break;
            }
            goto TR_000E;
        }

        private bool IsInside(ClipEdge2D edge, Vector2 test) => 
            !new bool?(this.IsLeftOf(edge, test)).Value;

        private bool IsLeftOf(ClipEdge2D edge, Vector2 test)
        {
            Vector2 vector = edge.to - edge.from;
            Vector2 vector2 = test - edge.to;
            double num = (vector.x * vector2.y) - (vector.y * vector2.x);
            return ((num >= 0.0) ? ((num <= 0.0) || true) : false);
        }
    }
}

