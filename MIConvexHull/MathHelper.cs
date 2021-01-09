namespace MIConvexHull
{
    using System;
    using System.Collections.Generic;

    internal class MathHelper
    {
        private readonly int Dimension;
        private double[] PositionData;
        private double[] ntX;
        private double[] ntY;
        private double[] ntZ;
        private double[] nDNormalHelperVector;
        private double[] nDMatrix;
        private int[] matrixPivots;

        public MathHelper(int dimension, double[] positions)
        {
            this.PositionData = positions;
            this.Dimension = dimension;
            this.ntX = new double[this.Dimension];
            this.ntY = new double[this.Dimension];
            this.ntZ = new double[this.Dimension];
            this.nDNormalHelperVector = new double[this.Dimension];
            this.nDMatrix = new double[this.Dimension * this.Dimension];
            this.matrixPivots = new int[this.Dimension];
        }

        public bool CalculateFacePlane(ConvexFaceInternal face, double[] center)
        {
            int[] vertices = face.Vertices;
            double[] normal = face.Normal;
            this.FindNormalVector(vertices, normal);
            if (double.IsNaN(normal[0]))
            {
                return false;
            }
            double num = 0.0;
            double num2 = 0.0;
            int num3 = vertices[0] * this.Dimension;
            for (int i = 0; i < this.Dimension; i++)
            {
                double num5 = normal[i];
                num += num5 * this.PositionData[num3 + i];
                num2 += num5 * center[i];
            }
            face.Offset = -num;
            if ((num2 - num) <= 0.0)
            {
                face.IsNormalFlipped = false;
            }
            else
            {
                int index = 0;
                while (true)
                {
                    if (index >= this.Dimension)
                    {
                        face.Offset = num;
                        face.IsNormalFlipped = true;
                        break;
                    }
                    normal[index] = -normal[index];
                    index++;
                }
            }
            return true;
        }

        private static double DeterminantDestructive(SimplexVolumeBuffer buff)
        {
            double[] data = buff.Data;
            switch (buff.Dimension)
            {
                case 0:
                    return 0.0;

                case 1:
                    return data[0];

                case 2:
                    return ((data[0] * data[3]) - (data[1] * data[2]));

                case 3:
                    return (((((((data[0] * data[4]) * data[8]) + ((data[1] * data[5]) * data[6])) + ((data[2] * data[3]) * data[7])) - ((data[0] * data[5]) * data[7])) - ((data[1] * data[3]) * data[8])) - ((data[2] * data[4]) * data[6]));
            }
            int[] pivots = buff.Pivots;
            int dimension = buff.Dimension;
            LUFactor(data, dimension, pivots, buff.Helper);
            double num3 = 1.0;
            for (int i = 0; i < pivots.Length; i++)
            {
                num3 *= data[(dimension * i) + i];
                if (pivots[i] != i)
                {
                    num3 *= -1.0;
                }
            }
            return num3;
        }

        private unsafe void FindNormal(int[] vertices, double[] normal)
        {
            int[] matrixPivots = this.matrixPivots;
            double[] nDMatrix = this.nDMatrix;
            double d = 0.0;
            int index = 0;
            while (index < this.Dimension)
            {
                int num3 = 0;
                while (true)
                {
                    if (num3 >= this.Dimension)
                    {
                        LUFactor(nDMatrix, this.Dimension, matrixPivots, this.nDNormalHelperVector);
                        double num6 = 1.0;
                        int num7 = 0;
                        while (true)
                        {
                            if (num7 >= this.Dimension)
                            {
                                normal[index] = num6;
                                d += num6 * num6;
                                index++;
                                break;
                            }
                            num6 = (matrixPivots[num7] == num7) ? (num6 * nDMatrix[(this.Dimension * num7) + num7]) : (num6 * -nDMatrix[(this.Dimension * num7) + num7]);
                            num7++;
                        }
                        break;
                    }
                    int num4 = vertices[num3] * this.Dimension;
                    int num5 = 0;
                    while (true)
                    {
                        if (num5 >= this.Dimension)
                        {
                            num3++;
                            break;
                        }
                        nDMatrix[(this.Dimension * num5) + num3] = (num5 != index) ? this.PositionData[num4 + num5] : 1.0;
                        num5++;
                    }
                }
            }
            double num8 = 1.0 / Math.Sqrt(d);
            for (int i = 0; i < normal.Length; i++)
            {
                double* numPtr1 = &(normal[i]);
                numPtr1[0] *= num8;
            }
        }

        public void FindNormalVector(int[] vertices, double[] normalData)
        {
            switch (this.Dimension)
            {
                case 2:
                    this.FindNormalVector2D(vertices, normalData);
                    break;

                case 3:
                    this.FindNormalVector3D(vertices, normalData);
                    break;

                case 4:
                    this.FindNormalVector4D(vertices, normalData);
                    break;

                default:
                    this.FindNormalVectorND(vertices, normalData);
                    break;
            }
        }

        private void FindNormalVector2D(int[] vertices, double[] normal)
        {
            this.SubtractFast(vertices[1], vertices[0], this.ntX);
            double[] ntX = this.ntX;
            double num = -ntX[1];
            double num2 = ntX[0];
            double num4 = 1.0 / Math.Sqrt((num * num) + (num2 * num2));
            normal[0] = num4 * num;
            normal[1] = num4 * num2;
        }

        private void FindNormalVector3D(int[] vertices, double[] normal)
        {
            this.SubtractFast(vertices[1], vertices[0], this.ntX);
            this.SubtractFast(vertices[2], vertices[1], this.ntY);
            double[] ntX = this.ntX;
            double[] ntY = this.ntY;
            double num = (ntX[1] * ntY[2]) - (ntX[2] * ntY[1]);
            double num2 = (ntX[2] * ntY[0]) - (ntX[0] * ntY[2]);
            double num3 = (ntX[0] * ntY[1]) - (ntX[1] * ntY[0]);
            double num5 = 1.0 / Math.Sqrt(((num * num) + (num2 * num2)) + (num3 * num3));
            normal[0] = num5 * num;
            normal[1] = num5 * num2;
            normal[2] = num5 * num3;
        }

        private void FindNormalVector4D(int[] vertices, double[] normal)
        {
            this.SubtractFast(vertices[1], vertices[0], this.ntX);
            this.SubtractFast(vertices[2], vertices[1], this.ntY);
            this.SubtractFast(vertices[3], vertices[2], this.ntZ);
            double[] ntX = this.ntX;
            double[] ntY = this.ntY;
            double[] ntZ = this.ntZ;
            double num = ((ntX[3] * ((ntY[2] * ntZ[1]) - (ntY[1] * ntZ[2]))) + (ntX[2] * ((ntY[1] * ntZ[3]) - (ntY[3] * ntZ[1])))) + (ntX[1] * ((ntY[3] * ntZ[2]) - (ntY[2] * ntZ[3])));
            double num2 = ((ntX[3] * ((ntY[0] * ntZ[2]) - (ntY[2] * ntZ[0]))) + (ntX[2] * ((ntY[3] * ntZ[0]) - (ntY[0] * ntZ[3])))) + (ntX[0] * ((ntY[2] * ntZ[3]) - (ntY[3] * ntZ[2])));
            double num3 = ((ntX[3] * ((ntY[1] * ntZ[0]) - (ntY[0] * ntZ[1]))) + (ntX[1] * ((ntY[0] * ntZ[3]) - (ntY[3] * ntZ[0])))) + (ntX[0] * ((ntY[3] * ntZ[1]) - (ntY[1] * ntZ[3])));
            double num4 = ((ntX[2] * ((ntY[0] * ntZ[1]) - (ntY[1] * ntZ[0]))) + (ntX[1] * ((ntY[2] * ntZ[0]) - (ntY[0] * ntZ[2])))) + (ntX[0] * ((ntY[1] * ntZ[2]) - (ntY[2] * ntZ[1])));
            double num6 = 1.0 / Math.Sqrt((((num * num) + (num2 * num2)) + (num3 * num3)) + (num4 * num4));
            normal[0] = num6 * num;
            normal[1] = num6 * num2;
            normal[2] = num6 * num3;
            normal[3] = num6 * num4;
        }

        private unsafe void FindNormalVectorND(int[] vertices, double[] normal)
        {
            int[] matrixPivots = this.matrixPivots;
            double[] nDMatrix = this.nDMatrix;
            double d = 0.0;
            int index = 0;
            while (index < this.Dimension)
            {
                int num3 = 0;
                while (true)
                {
                    if (num3 >= this.Dimension)
                    {
                        LUFactor(nDMatrix, this.Dimension, matrixPivots, this.nDNormalHelperVector);
                        double num6 = 1.0;
                        int num7 = 0;
                        while (true)
                        {
                            if (num7 >= this.Dimension)
                            {
                                normal[index] = num6;
                                d += num6 * num6;
                                index++;
                                break;
                            }
                            num6 = (matrixPivots[num7] == num7) ? (num6 * nDMatrix[(this.Dimension * num7) + num7]) : (num6 * -nDMatrix[(this.Dimension * num7) + num7]);
                            num7++;
                        }
                        break;
                    }
                    int num4 = vertices[num3] * this.Dimension;
                    int num5 = 0;
                    while (true)
                    {
                        if (num5 >= this.Dimension)
                        {
                            num3++;
                            break;
                        }
                        nDMatrix[(this.Dimension * num3) + num5] = (num5 != index) ? this.PositionData[num4 + num5] : 1.0;
                        num5++;
                    }
                }
            }
            double num8 = 1.0 / Math.Sqrt(d);
            for (int i = 0; i < normal.Length; i++)
            {
                double* numPtr1 = &(normal[i]);
                numPtr1[0] *= num8;
            }
        }

        public static double GetSimplexVolume(ConvexFaceInternal cell, IList<IVertex> vertices, SimplexVolumeBuffer buffer)
        {
            int[] numArray = cell.Vertices;
            double[] position = vertices[numArray[0]].Position;
            double[] data = buffer.Data;
            int dimension = buffer.Dimension;
            double num2 = 1.0;
            int index = 1;
            while (index < numArray.Length)
            {
                num2 *= index + 1;
                double[] numArray4 = vertices[numArray[index]].Position;
                int num4 = 0;
                while (true)
                {
                    if (num4 >= numArray4.Length)
                    {
                        index++;
                        break;
                    }
                    data[((num4 * dimension) + index) - 1] = numArray4[num4] - position[num4];
                    num4++;
                }
            }
            return (Math.Abs(DeterminantDestructive(buffer)) / num2);
        }

        public double GetVertexDistance(int v, ConvexFaceInternal f)
        {
            double[] normal = f.Normal;
            int num = v * this.Dimension;
            double offset = f.Offset;
            for (int i = 0; i < normal.Length; i++)
            {
                offset += normal[i] * this.PositionData[num + i];
            }
            return offset;
        }

        public static double LengthSquared(double[] x)
        {
            double num = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double num3 = x[i];
                num += num3 * num3;
            }
            return num;
        }

        private static unsafe void LUFactor(double[] data, int order, int[] ipiv, double[] vecLUcolj)
        {
            for (int i = 0; i < order; i++)
            {
                ipiv[i] = i;
            }
            int index = 0;
            while (index < order)
            {
                int num3 = index * order;
                int num4 = num3 + index;
                int num5 = 0;
                while (true)
                {
                    if (num5 >= order)
                    {
                        int num6 = 0;
                        while (true)
                        {
                            if (num6 >= order)
                            {
                                int num11 = index;
                                int num12 = index + 1;
                                while (true)
                                {
                                    if (num12 >= order)
                                    {
                                        if (num11 != index)
                                        {
                                            int num13 = 0;
                                            while (true)
                                            {
                                                if (num13 >= order)
                                                {
                                                    ipiv[index] = num11;
                                                    break;
                                                }
                                                int num14 = num13 * order;
                                                int num15 = num14 + num11;
                                                int num16 = num14 + index;
                                                double num17 = data[num15];
                                                data[num15] = data[num16];
                                                data[num16] = num17;
                                                num13++;
                                            }
                                        }
                                        if ((index < order) & (data[num4] != 0.0))
                                        {
                                            for (int j = index + 1; j < order; j++)
                                            {
                                                double* numPtr2 = &(data[num3 + j]);
                                                numPtr2[0] /= data[num4];
                                            }
                                        }
                                        index++;
                                        break;
                                    }
                                    if (Math.Abs(vecLUcolj[num12]) > Math.Abs(vecLUcolj[num11]))
                                    {
                                        num11 = num12;
                                    }
                                    num12++;
                                }
                                break;
                            }
                            int num7 = Math.Min(num6, index);
                            double num8 = 0.0;
                            int num9 = 0;
                            while (true)
                            {
                                if (num9 >= num7)
                                {
                                    double* numPtr1 = &(vecLUcolj[num6]);
                                    data[num3 + num6] = numPtr1[0] -= num8;
                                    num6++;
                                    break;
                                }
                                num8 += data[(num9 * order) + num6] * vecLUcolj[num9];
                                num9++;
                            }
                        }
                        break;
                    }
                    vecLUcolj[num5] = data[num3 + num5];
                    num5++;
                }
            }
        }

        public void SubtractFast(int x, int y, double[] target)
        {
            int num = x * this.Dimension;
            int num2 = y * this.Dimension;
            for (int i = 0; i < target.Length; i++)
            {
                target[i] = this.PositionData[num + i] - this.PositionData[num2 + i];
            }
        }

        public class SimplexVolumeBuffer
        {
            public int Dimension;
            public double[] Data;
            public double[] Helper;
            public int[] Pivots;

            public SimplexVolumeBuffer(int dimension)
            {
                this.Dimension = dimension;
                this.Data = new double[dimension * dimension];
                this.Helper = new double[dimension];
                this.Pivots = new int[dimension];
            }
        }
    }
}

