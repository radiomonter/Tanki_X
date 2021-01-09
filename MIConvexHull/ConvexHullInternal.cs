namespace MIConvexHull
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ConvexHullInternal
    {
        internal readonly int Dimension;
        private readonly bool IsLifted;
        private readonly double PlaneDistanceTolerance;
        private IVertex[] Vertices;
        private double[] Positions;
        private bool[] VertexMarks;
        internal ConvexFaceInternal[] FacePool;
        internal bool[] AffectedFaceFlags;
        private int ConvexHullSize;
        private FaceList UnprocessedFaces;
        private IndexBuffer ConvexFaces;
        private int CurrentVertex;
        private double MaxDistance;
        private int FurthestVertex;
        private double[] Center;
        private int[] UpdateBuffer;
        private int[] UpdateIndices;
        private IndexBuffer TraverseStack;
        private IndexBuffer EmptyBuffer;
        private IndexBuffer BeyondBuffer;
        private IndexBuffer AffectedFaceBuffer;
        private SimpleList<DeferredFace> ConeFaceBuffer;
        private HashSet<int> SingularVertices;
        private ConnectorList[] ConnectorTable;
        private const int ConnectorTableSize = 0x7e1;
        private MIConvexHull.ObjectManager ObjectManager;
        private MIConvexHull.MathHelper MathHelper;

        private ConvexHullInternal(IVertex[] vertices, bool lift, ConvexHullComputationConfig config)
        {
            if ((config.PointTranslationType != PointTranslationType.None) && (config.PointTranslationGenerator == null))
            {
                throw new InvalidOperationException("PointTranslationGenerator cannot be null if PointTranslationType is enabled.");
            }
            this.IsLifted = lift;
            this.Vertices = vertices;
            this.PlaneDistanceTolerance = config.PlaneDistanceTolerance;
            this.Dimension = this.DetermineDimension();
            if (this.Dimension < 2)
            {
                throw new InvalidOperationException("Dimension of the input must be 2 or greater.");
            }
            if (lift)
            {
                this.Dimension++;
            }
            this.InitializeData(config);
        }

        private void CommitCone()
        {
            int num = 0;
            while (num < this.ConeFaceBuffer.Count)
            {
                DeferredFace face = this.ConeFaceBuffer[num];
                ConvexFaceInternal internal2 = face.Face;
                ConvexFaceInternal pivot = face.Pivot;
                ConvexFaceInternal oldFace = face.OldFace;
                int faceIndex = face.FaceIndex;
                internal2.AdjacentFaces[faceIndex] = pivot.Index;
                pivot.AdjacentFaces[face.PivotIndex] = internal2.Index;
                int edgeIndex = 0;
                while (true)
                {
                    if (edgeIndex >= this.Dimension)
                    {
                        if (pivot.VerticesBeyond.Count == 0)
                        {
                            this.FindBeyondVertices(internal2, oldFace.VerticesBeyond);
                        }
                        else if (pivot.VerticesBeyond.Count < oldFace.VerticesBeyond.Count)
                        {
                            this.FindBeyondVertices(internal2, pivot.VerticesBeyond, oldFace.VerticesBeyond);
                        }
                        else
                        {
                            this.FindBeyondVertices(internal2, oldFace.VerticesBeyond, pivot.VerticesBeyond);
                        }
                        if (internal2.VerticesBeyond.Count != 0)
                        {
                            this.UnprocessedFaces.Add(internal2);
                        }
                        else
                        {
                            this.ConvexFaces.Add(internal2.Index);
                            this.UnprocessedFaces.Remove(internal2);
                            this.ObjectManager.DepositVertexBuffer(internal2.VerticesBeyond);
                            internal2.VerticesBeyond = this.EmptyBuffer;
                        }
                        this.ObjectManager.DepositDeferredFace(face);
                        num++;
                        break;
                    }
                    if (edgeIndex != faceIndex)
                    {
                        FaceConnector connector = this.ObjectManager.GetConnector();
                        connector.Update(internal2, edgeIndex, this.Dimension);
                        this.ConnectFace(connector);
                    }
                    edgeIndex++;
                }
            }
            for (int i = 0; i < this.AffectedFaceBuffer.Count; i++)
            {
                int index = this.AffectedFaceBuffer[i];
                this.UnprocessedFaces.Remove(this.FacePool[index]);
                this.ObjectManager.DepositFace(index);
            }
        }

        private void ConnectFace(FaceConnector connector)
        {
            uint index = connector.HashCode % 0x7e1;
            ConnectorList list = this.ConnectorTable[index];
            for (FaceConnector connector2 = list.First; connector2 != null; connector2 = connector2.Next)
            {
                if (FaceConnector.AreConnectable(connector, connector2, this.Dimension))
                {
                    list.Remove(connector2);
                    FaceConnector.Connect(connector2, connector);
                    connector2.Face = null;
                    connector.Face = null;
                    this.ObjectManager.DepositConnector(connector2);
                    this.ObjectManager.DepositConnector(connector);
                    return;
                }
            }
            list.Add(connector);
        }

        private bool CreateCone()
        {
            int currentVertex = this.CurrentVertex;
            this.ConeFaceBuffer.Clear();
            int num2 = 0;
            while (num2 < this.AffectedFaceBuffer.Count)
            {
                int index = this.AffectedFaceBuffer[num2];
                ConvexFaceInternal oldFace = this.FacePool[index];
                int num4 = 0;
                int num5 = 0;
                while (true)
                {
                    if (num5 >= this.Dimension)
                    {
                        int num7 = 0;
                        while (true)
                        {
                            if (num7 >= num4)
                            {
                                num2++;
                                break;
                            }
                            ConvexFaceInternal pivot = this.FacePool[this.UpdateBuffer[num7]];
                            int pivotIndex = 0;
                            int[] adjacentFaces = pivot.AdjacentFaces;
                            int num9 = 0;
                            while (true)
                            {
                                if (num9 < adjacentFaces.Length)
                                {
                                    if (index != adjacentFaces[num9])
                                    {
                                        num9++;
                                        continue;
                                    }
                                    pivotIndex = num9;
                                }
                                int num10 = this.UpdateIndices[num7];
                                int face = this.ObjectManager.GetFace();
                                ConvexFaceInternal internal4 = this.FacePool[face];
                                int[] vertices = internal4.Vertices;
                                int num13 = 0;
                                while (true)
                                {
                                    if (num13 >= this.Dimension)
                                    {
                                        int num14;
                                        if (currentVertex < vertices[num10])
                                        {
                                            num14 = 0;
                                            for (int i = num10 - 1; i >= 0; i--)
                                            {
                                                if (vertices[i] <= currentVertex)
                                                {
                                                    num14 = i + 1;
                                                    break;
                                                }
                                                vertices[i + 1] = vertices[i];
                                            }
                                        }
                                        else
                                        {
                                            num14 = this.Dimension - 1;
                                            for (int i = num10 + 1; i < this.Dimension; i++)
                                            {
                                                if (vertices[i] >= currentVertex)
                                                {
                                                    num14 = i - 1;
                                                    break;
                                                }
                                                vertices[i - 1] = vertices[i];
                                            }
                                        }
                                        vertices[num14] = this.CurrentVertex;
                                        if (!this.MathHelper.CalculateFacePlane(internal4, this.Center))
                                        {
                                            return false;
                                        }
                                        this.ConeFaceBuffer.Add(this.MakeDeferredFace(internal4, num14, pivot, pivotIndex, oldFace));
                                        num7++;
                                        break;
                                    }
                                    vertices[num13] = oldFace.Vertices[num13];
                                    num13++;
                                }
                                break;
                            }
                        }
                        break;
                    }
                    int num6 = oldFace.AdjacentFaces[num5];
                    if (!this.AffectedFaceFlags[num6])
                    {
                        this.UpdateBuffer[num4] = num6;
                        this.UpdateIndices[num4] = num5;
                        num4++;
                    }
                    num5++;
                }
            }
            return true;
        }

        private int[] CreateInitialHull(List<int> initialPoints)
        {
            int[] numArray = new int[this.Dimension + 1];
            int index = 0;
            while (index < (this.Dimension + 1))
            {
                int[] array = new int[this.Dimension];
                int num2 = 0;
                int num3 = 0;
                while (true)
                {
                    if (num2 > this.Dimension)
                    {
                        ConvexFaceInternal face = this.FacePool[this.ObjectManager.GetFace()];
                        face.Vertices = array;
                        Array.Sort<int>(array);
                        this.MathHelper.CalculateFacePlane(face, this.Center);
                        numArray[index] = face.Index;
                        index++;
                        break;
                    }
                    if (index != num2)
                    {
                        array[num3++] = initialPoints[num2];
                    }
                    num2++;
                }
            }
            int num4 = 0;
            while (num4 < this.Dimension)
            {
                int num5 = num4 + 1;
                while (true)
                {
                    if (num5 >= (this.Dimension + 1))
                    {
                        num4++;
                        break;
                    }
                    this.UpdateAdjacency(this.FacePool[numArray[num4]], this.FacePool[numArray[num5]]);
                    num5++;
                }
            }
            return numArray;
        }

        private int DetermineDimension()
        {
            Random random = new Random();
            int length = this.Vertices.Length;
            List<int> list = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(this.Vertices[random.Next(length)].Position.Length);
            }
            int num3 = ((IEnumerable<int>) list).Min();
            if (num3 != ((IEnumerable<int>) list).Max())
            {
                throw new ArgumentException("Invalid input data (non-uniform dimension).");
            }
            return num3;
        }

        private void FindBeyondVertices(ConvexFaceInternal face)
        {
            IndexBuffer verticesBeyond = face.VerticesBeyond;
            this.MaxDistance = double.NegativeInfinity;
            this.FurthestVertex = 0;
            int length = this.Vertices.Length;
            for (int i = 0; i < length; i++)
            {
                if (!this.VertexMarks[i])
                {
                    this.IsBeyond(face, verticesBeyond, i);
                }
            }
            face.FurthestVertex = this.FurthestVertex;
        }

        private void FindBeyondVertices(ConvexFaceInternal face, IndexBuffer beyond)
        {
            IndexBuffer beyondBuffer = this.BeyondBuffer;
            this.MaxDistance = double.NegativeInfinity;
            this.FurthestVertex = 0;
            for (int i = 0; i < beyond.Count; i++)
            {
                int v = beyond[i];
                if (v != this.CurrentVertex)
                {
                    this.IsBeyond(face, beyondBuffer, v);
                }
            }
            face.FurthestVertex = this.FurthestVertex;
            IndexBuffer verticesBeyond = face.VerticesBeyond;
            face.VerticesBeyond = beyondBuffer;
            if (verticesBeyond.Count > 0)
            {
                verticesBeyond.Clear();
            }
            this.BeyondBuffer = verticesBeyond;
        }

        private void FindBeyondVertices(ConvexFaceInternal face, IndexBuffer beyond, IndexBuffer beyond1)
        {
            int num;
            IndexBuffer beyondBuffer = this.BeyondBuffer;
            this.MaxDistance = double.NegativeInfinity;
            this.FurthestVertex = 0;
            for (int i = 0; i < beyond1.Count; i++)
            {
                this.VertexMarks[beyond1[i]] = true;
            }
            this.VertexMarks[this.CurrentVertex] = false;
            for (int j = 0; j < beyond.Count; j++)
            {
                num = beyond[j];
                if (num != this.CurrentVertex)
                {
                    this.VertexMarks[num] = false;
                    this.IsBeyond(face, beyondBuffer, num);
                }
            }
            for (int k = 0; k < beyond1.Count; k++)
            {
                num = beyond1[k];
                if (this.VertexMarks[num])
                {
                    this.IsBeyond(face, beyondBuffer, num);
                }
            }
            face.FurthestVertex = this.FurthestVertex;
            IndexBuffer verticesBeyond = face.VerticesBeyond;
            face.VerticesBeyond = beyondBuffer;
            if (verticesBeyond.Count > 0)
            {
                verticesBeyond.Clear();
            }
            this.BeyondBuffer = verticesBeyond;
        }

        private void FindConvexHull()
        {
            this.InitConvexHull();
            while (this.UnprocessedFaces.First != null)
            {
                ConvexFaceInternal first = this.UnprocessedFaces.First;
                this.CurrentVertex = first.FurthestVertex;
                this.UpdateCenter();
                this.TagAffectedFaces(first);
                if (!this.SingularVertices.Contains(this.CurrentVertex) && this.CreateCone())
                {
                    this.CommitCone();
                }
                else
                {
                    this.HandleSingular();
                }
                int count = this.AffectedFaceBuffer.Count;
                for (int i = 0; i < count; i++)
                {
                    this.AffectedFaceFlags[this.AffectedFaceBuffer[i]] = false;
                }
            }
        }

        private List<int> FindExtremes()
        {
            List<int> collection = new List<int>(2 * this.Dimension);
            int length = this.Vertices.Length;
            int i = 0;
            while (i < this.Dimension)
            {
                double maxValue = double.MaxValue;
                double minValue = double.MinValue;
                int item = 0;
                int num6 = 0;
                int v = 0;
                while (true)
                {
                    if (v >= length)
                    {
                        if (item == num6)
                        {
                            collection.Add(item);
                        }
                        else
                        {
                            collection.Add(item);
                            collection.Add(num6);
                        }
                        i++;
                        break;
                    }
                    double coordinate = this.GetCoordinate(v, i);
                    double num9 = maxValue - coordinate;
                    if (num9 >= 0.0)
                    {
                        if (num9 >= this.PlaneDistanceTolerance)
                        {
                            maxValue = coordinate;
                            item = v;
                        }
                        else if (this.LexCompare(v, item) < 0)
                        {
                            maxValue = coordinate;
                            item = v;
                        }
                    }
                    num9 = coordinate - minValue;
                    if (num9 >= 0.0)
                    {
                        if (num9 >= this.PlaneDistanceTolerance)
                        {
                            minValue = coordinate;
                            num6 = v;
                        }
                        else if (this.LexCompare(v, num6) > 0)
                        {
                            minValue = coordinate;
                            num6 = v;
                        }
                    }
                    v++;
                }
            }
            HashSet<int> source = new HashSet<int>(collection);
            if (source.Count <= this.Dimension)
            {
                for (int j = 0; (j < length) && (source.Count <= this.Dimension); j++)
                {
                    source.Add(j);
                }
            }
            return source.ToList<int>();
        }

        private List<int> FindInitialPoints(List<int> extremes)
        {
            List<int> initialPoints = new List<int>();
            int item = -1;
            int num2 = -1;
            double num3 = 0.0;
            double[] target = new double[this.Dimension];
            int num4 = 0;
            while (num4 < (extremes.Count - 1))
            {
                int x = extremes[num4];
                int num6 = num4 + 1;
                while (true)
                {
                    if (num6 >= extremes.Count)
                    {
                        num4++;
                        break;
                    }
                    int y = extremes[num6];
                    this.MathHelper.SubtractFast(x, y, target);
                    double num8 = MIConvexHull.MathHelper.LengthSquared(target);
                    if (num8 > num3)
                    {
                        item = x;
                        num2 = y;
                        num3 = num8;
                    }
                    num6++;
                }
            }
            initialPoints.Add(item);
            initialPoints.Add(num2);
            int num9 = 2;
            while (num9 <= this.Dimension)
            {
                double negativeInfinity = double.NegativeInfinity;
                int num11 = -1;
                int num12 = 0;
                while (true)
                {
                    if (num12 >= extremes.Count)
                    {
                        if (num11 >= 0)
                        {
                            initialPoints.Add(num11);
                        }
                        else
                        {
                            int length = this.Vertices.Length;
                            int num16 = 0;
                            while (true)
                            {
                                if (num16 >= length)
                                {
                                    if (num11 >= 0)
                                    {
                                        initialPoints.Add(num11);
                                    }
                                    else
                                    {
                                        this.ThrowSingular();
                                    }
                                    break;
                                }
                                if (!initialPoints.Contains(num16))
                                {
                                    double squaredDistanceSum = this.GetSquaredDistanceSum(num16, initialPoints);
                                    if (squaredDistanceSum > negativeInfinity)
                                    {
                                        negativeInfinity = squaredDistanceSum;
                                        num11 = num16;
                                    }
                                }
                                num16++;
                            }
                        }
                        num9++;
                        break;
                    }
                    int num13 = extremes[num12];
                    if (!initialPoints.Contains(num13))
                    {
                        double squaredDistanceSum = this.GetSquaredDistanceSum(num13, initialPoints);
                        if (squaredDistanceSum > negativeInfinity)
                        {
                            negativeInfinity = squaredDistanceSum;
                            num11 = num13;
                        }
                    }
                    num12++;
                }
            }
            return initialPoints;
        }

        private TFace[] GetConvexFaces<TVertex, TFace>() where TVertex: IVertex where TFace: ConvexFace<TVertex, TFace>, new()
        {
            IndexBuffer convexFaces = this.ConvexFaces;
            int count = convexFaces.Count;
            TFace[] localArray = new TFace[count];
            int index = 0;
            while (index < count)
            {
                ConvexFaceInternal internal2 = this.FacePool[convexFaces[index]];
                TVertex[] localArray2 = new TVertex[this.Dimension];
                int num3 = 0;
                while (true)
                {
                    if (num3 >= this.Dimension)
                    {
                        TFace local = Activator.CreateInstance<TFace>();
                        local.Vertices = localArray2;
                        local.Adjacency = new TFace[this.Dimension];
                        local.Normal = !this.IsLifted ? internal2.Normal : null;
                        localArray[index] = local;
                        internal2.Tag = index;
                        index++;
                        break;
                    }
                    localArray2[num3] = (TVertex) this.Vertices[internal2.Vertices[num3]];
                    num3++;
                }
            }
            int num4 = 0;
            while (num4 < count)
            {
                ConvexFaceInternal internal3 = this.FacePool[convexFaces[num4]];
                TFace local2 = localArray[num4];
                int num5 = 0;
                while (true)
                {
                    if (num5 >= this.Dimension)
                    {
                        if (internal3.IsNormalFlipped)
                        {
                            TVertex local3 = local2.Vertices[0];
                            local2.Vertices[0] = local2.Vertices[this.Dimension - 1];
                            local2.Vertices[this.Dimension - 1] = local3;
                            TFace local4 = local2.Adjacency[0];
                            local2.Adjacency[0] = local2.Adjacency[this.Dimension - 1];
                            local2.Adjacency[this.Dimension - 1] = local4;
                        }
                        num4++;
                        break;
                    }
                    if (internal3.AdjacentFaces[num5] >= 0)
                    {
                        local2.Adjacency[num5] = localArray[this.FacePool[internal3.AdjacentFaces[num5]].Tag];
                    }
                    num5++;
                }
            }
            return localArray;
        }

        internal static ConvexHull<TVertex, TFace> GetConvexHull<TVertex, TFace>(IList<TVertex> data, ConvexHullComputationConfig config) where TVertex: IVertex where TFace: ConvexFace<TVertex, TFace>, new()
        {
            config ??= new ConvexHullComputationConfig();
            IVertex[] vertices = new IVertex[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                vertices[i] = data[i];
            }
            ConvexHullInternal internal2 = new ConvexHullInternal(vertices, false, config);
            internal2.FindConvexHull();
            return new ConvexHull<TVertex, TFace> { 
                Points = internal2.GetHullVertices<TVertex>(data),
                Faces = internal2.GetConvexFaces<TVertex, TFace>()
            };
        }

        private double GetCoordinate(int v, int i) => 
            this.Positions[(v * this.Dimension) + i];

        private TVertex[] GetHullVertices<TVertex>(IList<TVertex> data)
        {
            int count = this.ConvexFaces.Count;
            int num2 = 0;
            int length = this.Vertices.Length;
            for (int i = 0; i < length; i++)
            {
                this.VertexMarks[i] = false;
            }
            int num5 = 0;
            while (num5 < count)
            {
                int[] vertices = this.FacePool[this.ConvexFaces[num5]].Vertices;
                int index = 0;
                while (true)
                {
                    if (index >= vertices.Length)
                    {
                        num5++;
                        break;
                    }
                    int num7 = vertices[index];
                    if (!this.VertexMarks[num7])
                    {
                        this.VertexMarks[num7] = true;
                        num2++;
                    }
                    index++;
                }
            }
            TVertex[] localArray = new TVertex[num2];
            for (int j = 0; j < length; j++)
            {
                if (this.VertexMarks[j])
                {
                    localArray[--num2] = data[j];
                }
            }
            return localArray;
        }

        private double GetSquaredDistanceSum(int pivot, List<int> initialPoints)
        {
            int count = initialPoints.Count;
            double num2 = 0.0;
            int num3 = 0;
            while (num3 < count)
            {
                int v = initialPoints[num3];
                int i = 0;
                while (true)
                {
                    if (i >= this.Dimension)
                    {
                        num3++;
                        break;
                    }
                    double num6 = this.GetCoordinate(v, i) - this.GetCoordinate(pivot, i);
                    num2 += num6 * num6;
                    i++;
                }
            }
            return num2;
        }

        private void HandleSingular()
        {
            this.RollbackCenter();
            this.SingularVertices.Add(this.CurrentVertex);
            int num = 0;
            while (num < this.AffectedFaceBuffer.Count)
            {
                ConvexFaceInternal face = this.FacePool[this.AffectedFaceBuffer[num]];
                IndexBuffer verticesBeyond = face.VerticesBeyond;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= verticesBeyond.Count)
                    {
                        this.ConvexFaces.Add(face.Index);
                        this.UnprocessedFaces.Remove(face);
                        this.ObjectManager.DepositVertexBuffer(face.VerticesBeyond);
                        face.VerticesBeyond = this.EmptyBuffer;
                        num++;
                        break;
                    }
                    this.SingularVertices.Add(verticesBeyond[num2]);
                    num2++;
                }
            }
        }

        private void InitConvexHull()
        {
            if (this.Vertices.Length >= this.Dimension)
            {
                if (this.Vertices.Length == this.Dimension)
                {
                    this.InitSingle();
                }
                else
                {
                    List<int> extremes = this.FindExtremes();
                    List<int> initialPoints = this.FindInitialPoints(extremes);
                    foreach (int num in initialPoints)
                    {
                        this.CurrentVertex = num;
                        this.UpdateCenter();
                        this.VertexMarks[num] = true;
                    }
                    foreach (int num2 in this.CreateInitialHull(initialPoints))
                    {
                        ConvexFaceInternal face = this.FacePool[num2];
                        this.FindBeyondVertices(face);
                        if (face.VerticesBeyond.Count == 0)
                        {
                            this.ConvexFaces.Add(face.Index);
                        }
                        else
                        {
                            this.UnprocessedFaces.Add(face);
                        }
                    }
                    foreach (int num4 in initialPoints)
                    {
                        this.VertexMarks[num4] = false;
                    }
                }
            }
        }

        private void InitializeData(ConvexHullComputationConfig config)
        {
            this.UnprocessedFaces = new FaceList();
            this.ConvexFaces = new IndexBuffer();
            this.FacePool = new ConvexFaceInternal[(this.Dimension + 1) * 10];
            this.AffectedFaceFlags = new bool[(this.Dimension + 1) * 10];
            this.ObjectManager = new MIConvexHull.ObjectManager(this);
            this.Center = new double[this.Dimension];
            this.TraverseStack = new IndexBuffer();
            this.UpdateBuffer = new int[this.Dimension];
            this.UpdateIndices = new int[this.Dimension];
            this.EmptyBuffer = new IndexBuffer();
            this.AffectedFaceBuffer = new IndexBuffer();
            this.ConeFaceBuffer = new SimpleList<DeferredFace>();
            this.SingularVertices = new HashSet<int>();
            this.BeyondBuffer = new IndexBuffer();
            this.ConnectorTable = new ConnectorList[0x7e1];
            for (int i = 0; i < 0x7e1; i++)
            {
                this.ConnectorTable[i] = new ConnectorList();
            }
            this.VertexMarks = new bool[this.Vertices.Length];
            this.InitializePositions(config);
            this.MathHelper = new MIConvexHull.MathHelper(this.Dimension, this.Positions);
        }

        private void InitializePositions(ConvexHullComputationConfig config)
        {
            this.Positions = new double[this.Vertices.Length * this.Dimension];
            int num = 0;
            if (!this.IsLifted)
            {
                Func<double> pointTranslationGenerator = config.PointTranslationGenerator;
                PointTranslationType pointTranslationType = config.PointTranslationType;
                if (pointTranslationType == PointTranslationType.None)
                {
                    IVertex[] vertices = this.Vertices;
                    int index = 0;
                    while (index < vertices.Length)
                    {
                        IVertex vertex3 = vertices[index];
                        int num12 = 0;
                        while (true)
                        {
                            if (num12 >= this.Dimension)
                            {
                                index++;
                                break;
                            }
                            this.Positions[num++] = vertex3.Position[num12];
                            num12++;
                        }
                    }
                }
                else if (pointTranslationType == PointTranslationType.TranslateInternal)
                {
                    IVertex[] vertices = this.Vertices;
                    int index = 0;
                    while (index < vertices.Length)
                    {
                        IVertex vertex4 = vertices[index];
                        int num14 = 0;
                        while (true)
                        {
                            if (num14 >= this.Dimension)
                            {
                                index++;
                                break;
                            }
                            this.Positions[num++] = vertex4.Position[num14] + pointTranslationGenerator();
                            num14++;
                        }
                    }
                }
            }
            else
            {
                int num2 = this.Dimension - 1;
                Func<double> pointTranslationGenerator = config.PointTranslationGenerator;
                PointTranslationType pointTranslationType = config.PointTranslationType;
                if (pointTranslationType == PointTranslationType.None)
                {
                    IVertex[] vertices = this.Vertices;
                    int index = 0;
                    while (index < vertices.Length)
                    {
                        IVertex vertex = vertices[index];
                        double num4 = 0.0;
                        int num5 = 0;
                        while (true)
                        {
                            if (num5 >= num2)
                            {
                                this.Positions[num++] = num4;
                                index++;
                                break;
                            }
                            double num6 = vertex.Position[num5];
                            this.Positions[num++] = num6;
                            num4 += num6 * num6;
                            num5++;
                        }
                    }
                }
                else if (pointTranslationType == PointTranslationType.TranslateInternal)
                {
                    IVertex[] vertices = this.Vertices;
                    int index = 0;
                    while (index < vertices.Length)
                    {
                        IVertex vertex2 = vertices[index];
                        double num8 = 0.0;
                        int num9 = 0;
                        while (true)
                        {
                            if (num9 >= num2)
                            {
                                this.Positions[num++] = num8;
                                index++;
                                break;
                            }
                            double num10 = vertex2.Position[num9] + pointTranslationGenerator();
                            this.Positions[num++] = num10;
                            num8 += num10 * num10;
                            num9++;
                        }
                    }
                }
            }
        }

        private unsafe void InitSingle()
        {
            int[] array = new int[this.Dimension];
            for (int i = 0; i < this.Vertices.Length; i++)
            {
                array[i] = i;
            }
            ConvexFaceInternal face = this.FacePool[this.ObjectManager.GetFace()];
            face.Vertices = array;
            Array.Sort<int>(array);
            this.MathHelper.CalculateFacePlane(face, this.Center);
            if (face.Normal[this.Dimension - 1] >= 0.0)
            {
                int index = 0;
                while (true)
                {
                    if (index >= this.Dimension)
                    {
                        face.Offset = -face.Offset;
                        face.IsNormalFlipped = !face.IsNormalFlipped;
                        break;
                    }
                    double* numPtr1 = &(face.Normal[index]);
                    numPtr1[0] *= -1.0;
                    index++;
                }
            }
            this.ConvexFaces.Add(face.Index);
        }

        private void IsBeyond(ConvexFaceInternal face, IndexBuffer beyondVertices, int v)
        {
            double vertexDistance = this.MathHelper.GetVertexDistance(v, face);
            if (vertexDistance >= this.PlaneDistanceTolerance)
            {
                if (vertexDistance > this.MaxDistance)
                {
                    if ((vertexDistance - this.MaxDistance) >= this.PlaneDistanceTolerance)
                    {
                        this.MaxDistance = vertexDistance;
                        this.FurthestVertex = v;
                    }
                    else if (this.LexCompare(v, this.FurthestVertex) > 0)
                    {
                        this.MaxDistance = vertexDistance;
                        this.FurthestVertex = v;
                    }
                }
                beyondVertices.Add(v);
            }
        }

        private int LexCompare(int u, int v)
        {
            int num = u * this.Dimension;
            int num2 = v * this.Dimension;
            for (int i = 0; i < this.Dimension; i++)
            {
                double num4 = this.Positions[num + i];
                double num5 = this.Positions[num2 + i];
                int num6 = num4.CompareTo(num5);
                if (num6 != 0)
                {
                    return num6;
                }
            }
            return 0;
        }

        private DeferredFace MakeDeferredFace(ConvexFaceInternal face, int faceIndex, ConvexFaceInternal pivot, int pivotIndex, ConvexFaceInternal oldFace)
        {
            DeferredFace deferredFace = this.ObjectManager.GetDeferredFace();
            deferredFace.Face = face;
            deferredFace.FaceIndex = faceIndex;
            deferredFace.Pivot = pivot;
            deferredFace.PivotIndex = pivotIndex;
            deferredFace.OldFace = oldFace;
            return deferredFace;
        }

        private unsafe void RollbackCenter()
        {
            for (int i = 0; i < this.Dimension; i++)
            {
                double* numPtr1 = &(this.Center[i]);
                numPtr1[0] *= this.ConvexHullSize;
            }
            this.ConvexHullSize--;
            double num2 = (this.ConvexHullSize <= 0) ? 0.0 : (1.0 / ((double) this.ConvexHullSize));
            int num3 = this.CurrentVertex * this.Dimension;
            for (int j = 0; j < this.Dimension; j++)
            {
                this.Center[j] = num2 * (this.Center[j] - this.Positions[num3 + j]);
            }
        }

        private void TagAffectedFaces(ConvexFaceInternal currentFace)
        {
            this.AffectedFaceBuffer.Clear();
            this.AffectedFaceBuffer.Add(currentFace.Index);
            this.TraverseAffectedFaces(currentFace.Index);
        }

        private void ThrowSingular()
        {
            throw new InvalidOperationException("Singular input data (i.e. trying to triangulate a data that contain a regular lattice of points) detected. Introducing some noise to the data might resolve the issue.");
        }

        private void TraverseAffectedFaces(int currentFace)
        {
            this.TraverseStack.Clear();
            this.TraverseStack.Push(currentFace);
            this.AffectedFaceFlags[currentFace] = true;
            while (this.TraverseStack.Count > 0)
            {
                ConvexFaceInternal internal2 = this.FacePool[this.TraverseStack.Pop()];
                for (int i = 0; i < this.Dimension; i++)
                {
                    int index = internal2.AdjacentFaces[i];
                    if (!this.AffectedFaceFlags[index] && (this.MathHelper.GetVertexDistance(this.CurrentVertex, this.FacePool[index]) >= this.PlaneDistanceTolerance))
                    {
                        this.AffectedFaceBuffer.Add(index);
                        this.AffectedFaceFlags[index] = true;
                        this.TraverseStack.Push(index);
                    }
                }
            }
        }

        private void UpdateAdjacency(ConvexFaceInternal l, ConvexFaceInternal r)
        {
            int num;
            int[] vertices = l.Vertices;
            int[] numArray2 = r.Vertices;
            for (num = 0; num < vertices.Length; num++)
            {
                this.VertexMarks[vertices[num]] = false;
            }
            for (num = 0; num < numArray2.Length; num++)
            {
                this.VertexMarks[numArray2[num]] = true;
            }
            num = 0;
            while ((num < vertices.Length) && this.VertexMarks[vertices[num]])
            {
                num++;
            }
            if (num != this.Dimension)
            {
                for (int i = num + 1; i < vertices.Length; i++)
                {
                    if (!this.VertexMarks[vertices[i]])
                    {
                        return;
                    }
                }
                l.AdjacentFaces[num] = r.Index;
                num = 0;
                while (num < vertices.Length)
                {
                    this.VertexMarks[vertices[num]] = false;
                    num++;
                }
                num = 0;
                while ((num < numArray2.Length) && !this.VertexMarks[numArray2[num]])
                {
                    num++;
                }
                r.AdjacentFaces[num] = l.Index;
            }
        }

        private unsafe void UpdateCenter()
        {
            for (int i = 0; i < this.Dimension; i++)
            {
                double* numPtr1 = &(this.Center[i]);
                numPtr1[0] *= this.ConvexHullSize;
            }
            this.ConvexHullSize++;
            double num2 = 1.0 / ((double) this.ConvexHullSize);
            int num3 = this.CurrentVertex * this.Dimension;
            for (int j = 0; j < this.Dimension; j++)
            {
                this.Center[j] = num2 * (this.Center[j] + this.Positions[num3 + j]);
            }
        }
    }
}

