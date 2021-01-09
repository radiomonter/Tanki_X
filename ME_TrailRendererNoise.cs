using System;
using System.Collections.Generic;
using UnityEngine;

public class ME_TrailRendererNoise : MonoBehaviour
{
    [Range(0.01f, 10f)]
    public float MinVertexDistance = 0.1f;
    public float VertexTime = 1f;
    public float TotalLifeTime = 3f;
    public bool SmoothCurves;
    public bool IsRibbon;
    public bool IsActive = true;
    [Range(0.001f, 10f)]
    public float Frequency = 1f;
    [Range(0.001f, 10f)]
    public float TimeScale = 0.1f;
    [Range(0.001f, 10f)]
    public float Amplitude = 1f;
    public float Gravity = 1f;
    public float TurbulenceStrength = 1f;
    public bool AutodestructWhenNotActive;
    private LineRenderer lineRenderer;
    private Transform t;
    private Vector3 prevPos;
    private List<Vector3> points = new List<Vector3>(500);
    private List<float> lifeTimes = new List<float>(500);
    private List<Vector3> velocities = new List<Vector3>(500);
    private float randomOffset;
    private List<Vector3> controlPoints = new List<Vector3>();
    private int curveCount;
    private const float MinimumSqrDistance = 0.01f;
    private const float DivisionThreshold = -0.99f;
    private const float SmoothCurvesScale = 0.5f;

    private void AddNewPoints()
    {
        if ((((this.t.position - this.prevPos).magnitude > this.MinVertexDistance) || (this.IsRibbon && (this.points.Count == 0))) || ((this.IsRibbon && (this.points.Count > 0)) && ((this.t.position - this.points[0]).magnitude > this.MinVertexDistance)))
        {
            this.prevPos = this.t.position;
            this.points.Insert(0, this.t.position);
            this.lifeTimes.Insert(0, this.VertexTime);
            this.velocities.Insert(0, Vector3.zero);
        }
    }

    public Vector3 CalculateBezierPoint(int curveIndex, float t)
    {
        int num = curveIndex * 3;
        Vector3 vector = this.controlPoints[num];
        return this.CalculateBezierPoint(t, vector, this.controlPoints[num + 1], this.controlPoints[num + 2], this.controlPoints[num + 3]);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float num = 1f - t;
        float num2 = t * t;
        float num3 = num * num;
        return (Vector3) (((((num3 * num) * p0) + (((3f * num3) * t) * p1)) + (((3f * num) * num2) * p2)) + ((num2 * t) * p3));
    }

    private void CalculateTurbuelence(Vector3 position, float speed, float scale, float height, float gravity, int index)
    {
        List<Vector3> list;
        int num5;
        float num = (Time.timeSinceLevelLoad * speed) + this.randomOffset;
        float x = (position.x * scale) + num;
        float num3 = ((position.y * scale) + num) + 10f;
        float y = ((position.z * scale) + num) + 25f;
        position.x = ((Mathf.PerlinNoise(num3, y) - 0.5f) * height) * Time.deltaTime;
        position.y = (((Mathf.PerlinNoise(x, y) - 0.5f) * height) * Time.deltaTime) - (gravity * Time.deltaTime);
        position.z = ((Mathf.PerlinNoise(x, num3) - 0.5f) * height) * Time.deltaTime;
        (list = this.points)[num5 = index] = list[num5] + (position * this.TurbulenceStrength);
    }

    private List<Vector3> FindDrawingPoints(int curveIndex)
    {
        List<Vector3> pointList = new List<Vector3> {
            this.CalculateBezierPoint(curveIndex, 0f),
            this.CalculateBezierPoint(curveIndex, 1f)
        };
        this.FindDrawingPoints(curveIndex, 0f, 1f, pointList, 1);
        return pointList;
    }

    private int FindDrawingPoints(int curveIndex, float t0, float t1, List<Vector3> pointList, int insertionIndex)
    {
        Vector3 vector = this.CalculateBezierPoint(curveIndex, t0);
        Vector3 vector2 = this.CalculateBezierPoint(curveIndex, t1);
        if ((vector - vector2).sqrMagnitude < 0.01f)
        {
            return 0;
        }
        float t = (t0 + t1) / 2f;
        Vector3 item = this.CalculateBezierPoint(curveIndex, t);
        if ((Vector3.Dot((vector - item).normalized, (vector2 - item).normalized) <= -0.99f) && (Mathf.Abs((float) (t - 0.5f)) >= 0.0001f))
        {
            return 0;
        }
        int num2 = 0 + this.FindDrawingPoints(curveIndex, t0, t, pointList, insertionIndex);
        pointList.Insert(insertionIndex + num2, item);
        num2++;
        return (num2 + this.FindDrawingPoints(curveIndex, t, t1, pointList, insertionIndex + num2));
    }

    public List<Vector3> GetDrawingPoints()
    {
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < this.curveCount; i++)
        {
            List<Vector3> collection = this.FindDrawingPoints(i);
            if (i != 0)
            {
                collection.RemoveAt(0);
            }
            list.AddRange(collection);
        }
        return list;
    }

    public void InterpolateBezier(List<Vector3> segmentPoints, float scale)
    {
        this.controlPoints.Clear();
        if (segmentPoints.Count >= 2)
        {
            for (int i = 0; i < segmentPoints.Count; i++)
            {
                if (i == 0)
                {
                    Vector3 item = segmentPoints[i];
                    Vector3 vector3 = segmentPoints[i + 1] - item;
                    Vector3 vector4 = item + (scale * vector3);
                    this.controlPoints.Add(item);
                    this.controlPoints.Add(vector4);
                }
                else if (i == (segmentPoints.Count - 1))
                {
                    Vector3 item = segmentPoints[i];
                    Vector3 vector7 = item - segmentPoints[i - 1];
                    Vector3 vector8 = item - (scale * vector7);
                    this.controlPoints.Add(vector8);
                    this.controlPoints.Add(item);
                }
                else
                {
                    Vector3 vector9 = segmentPoints[i - 1];
                    Vector3 item = segmentPoints[i];
                    Vector3 vector11 = segmentPoints[i + 1];
                    Vector3 normalized = (vector11 - vector9).normalized;
                    Vector3 vector14 = item - ((scale * normalized) * (item - vector9).magnitude);
                    Vector3 vector16 = item + ((scale * normalized) * (vector11 - item).magnitude);
                    this.controlPoints.Add(vector14);
                    this.controlPoints.Add(item);
                    this.controlPoints.Add(vector16);
                }
            }
            this.curveCount = (this.controlPoints.Count - 1) / 3;
        }
    }

    private void OnEnable()
    {
        this.points.Clear();
        this.lifeTimes.Clear();
        this.velocities.Clear();
    }

    private void Start()
    {
        this.lineRenderer = base.GetComponent<LineRenderer>();
        this.lineRenderer.useWorldSpace = true;
        this.t = base.transform;
        this.prevPos = this.t.position;
        this.points.Insert(0, this.t.position);
        this.lifeTimes.Insert(0, this.VertexTime);
        this.velocities.Insert(0, Vector3.zero);
        this.randomOffset = ((float) Random.Range(0, 0x989680)) / 1000000f;
    }

    private void Update()
    {
        if (this.IsActive)
        {
            this.AddNewPoints();
        }
        this.UpdatetPoints();
        if (this.SmoothCurves && (this.points.Count > 2))
        {
            this.UpdateLineRendererBezier();
        }
        else
        {
            this.UpdateLineRenderer();
        }
        if (this.AutodestructWhenNotActive && (!this.IsActive && (this.points.Count <= 1)))
        {
            Destroy(base.gameObject, this.TotalLifeTime);
        }
    }

    private void UpdateLineRenderer()
    {
        this.lineRenderer.positionCount = Mathf.Clamp(this.points.Count - 1, 0, 0x7fffffff);
        this.lineRenderer.SetPositions(this.points.ToArray());
    }

    private void UpdateLineRendererBezier()
    {
        if (this.SmoothCurves && (this.points.Count > 2))
        {
            this.InterpolateBezier(this.points, 0.5f);
            List<Vector3> drawingPoints = this.GetDrawingPoints();
            this.lineRenderer.positionCount = drawingPoints.Count - 1;
            this.lineRenderer.SetPositions(drawingPoints.ToArray());
        }
    }

    private void UpdatetPoints()
    {
        for (int i = 0; i < this.lifeTimes.Count; i++)
        {
            List<float> list;
            int num2;
            (list = this.lifeTimes)[num2 = i] = list[num2] - Time.deltaTime;
            if (this.lifeTimes[i] <= 0f)
            {
                int count = this.lifeTimes.Count - i;
                this.lifeTimes.RemoveRange(i, count);
                this.points.RemoveRange(i, count);
                this.velocities.RemoveRange(i, count);
                return;
            }
            this.CalculateTurbuelence(this.points[i], this.TimeScale, this.Frequency, this.Amplitude, this.Gravity, i);
        }
    }
}

