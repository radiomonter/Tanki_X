using System;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct P3D_Matrix
{
    public float m00;
    public float m10;
    public float m20;
    public float m01;
    public float m11;
    public float m21;
    public float m02;
    public float m12;
    public float m22;
    public static P3D_Matrix Identity =>
        new P3D_Matrix { 
            m00=1f,
            m10=0f,
            m20=0f,
            m01=0f,
            m11=1f,
            m21=0f,
            m02=0f,
            m12=0f,
            m22=1f
        };
    public static P3D_Matrix Translation(float x, float y) => 
        new P3D_Matrix { 
            m00 = 1f,
            m10 = 0f,
            m20 = 0f,
            m01 = 0f,
            m11 = 1f,
            m21 = 0f,
            m02 = x,
            m12 = y,
            m22 = 1f
        };

    public static P3D_Matrix Scaling(float x, float y) => 
        new P3D_Matrix { 
            m00 = x,
            m10 = 0f,
            m20 = 0f,
            m01 = 0f,
            m11 = y,
            m21 = 0f,
            m02 = 0f,
            m12 = 0f,
            m22 = 1f
        };

    public static P3D_Matrix Rotation(float a)
    {
        float num = Mathf.Sin(a);
        float num2 = Mathf.Cos(a);
        return new P3D_Matrix { 
            m00 = num2,
            m10 = -num,
            m20 = 0f,
            m01 = num,
            m11 = num2,
            m21 = 0f,
            m02 = 0f,
            m12 = 0f,
            m22 = 1f
        };
    }

    public P3D_Matrix Inverse
    {
        get
        {
            double num = ((this.m00 * ((this.m11 * this.m22) - (this.m21 * this.m12))) - (this.m01 * ((this.m10 * this.m22) - (this.m12 * this.m20)))) + (this.m02 * ((this.m10 * this.m21) - (this.m11 * this.m20)));
            if (num == 0.0)
            {
                return Identity;
            }
            float num2 = (float) (1.0 / num);
            return new P3D_Matrix { 
                m00 = ((this.m11 * this.m22) - (this.m21 * this.m12)) * num2,
                m10 = -((this.m10 * this.m22) - (this.m12 * this.m20)) * num2,
                m20 = ((this.m10 * this.m21) - (this.m20 * this.m11)) * num2,
                m01 = -((this.m01 * this.m22) - (this.m02 * this.m21)) * num2,
                m11 = ((this.m00 * this.m22) - (this.m02 * this.m20)) * num2,
                m12 = -((this.m00 * this.m12) - (this.m10 * this.m02)) * num2,
                m02 = ((this.m01 * this.m12) - (this.m02 * this.m11)) * num2,
                m22 = ((this.m00 * this.m11) - (this.m10 * this.m01)) * num2,
                m21 = -((this.m00 * this.m21) - (this.m20 * this.m01)) * num2
            };
        }
    }
    public UnityEngine.Matrix4x4 Matrix4x4
    {
        get
        {
            UnityEngine.Matrix4x4 identity = UnityEngine.Matrix4x4.identity;
            identity.m00 = this.m00;
            identity.m10 = this.m10;
            identity.m20 = this.m20;
            identity.m01 = this.m01;
            identity.m11 = this.m11;
            identity.m21 = this.m21;
            identity.m02 = this.m02;
            identity.m12 = this.m12;
            identity.m22 = this.m22;
            return identity;
        }
    }
    public Vector2 MultiplyPoint(Vector2 v)
    {
        Vector2 vector = new Vector2();
        vector.x = ((this.m00 * v.x) + (this.m01 * v.y)) + this.m02;
        vector.y = ((this.m10 * v.x) + (this.m11 * v.y)) + this.m12;
        return vector;
    }

    public Vector2 MultiplyPoint(float x, float y)
    {
        Vector2 vector = new Vector2();
        vector.x = ((this.m00 * x) + (this.m01 * y)) + this.m02;
        vector.y = ((this.m10 * x) + (this.m11 * y)) + this.m12;
        return vector;
    }

    public static P3D_Matrix operator *(P3D_Matrix lhs, P3D_Matrix rhs) => 
        new P3D_Matrix { 
            m00 = ((lhs.m00 * rhs.m00) + (lhs.m01 * rhs.m10)) + (lhs.m02 * rhs.m20),
            m01 = ((lhs.m00 * rhs.m01) + (lhs.m01 * rhs.m11)) + (lhs.m02 * rhs.m21),
            m02 = ((lhs.m00 * rhs.m02) + (lhs.m01 * rhs.m12)) + (lhs.m02 * rhs.m22),
            m10 = ((lhs.m10 * rhs.m00) + (lhs.m11 * rhs.m10)) + (lhs.m12 * rhs.m20),
            m11 = ((lhs.m10 * rhs.m01) + (lhs.m11 * rhs.m11)) + (lhs.m12 * rhs.m21),
            m12 = ((lhs.m10 * rhs.m02) + (lhs.m11 * rhs.m12)) + (lhs.m12 * rhs.m22),
            m20 = ((lhs.m20 * rhs.m00) + (lhs.m21 * rhs.m10)) + (lhs.m22 * rhs.m20),
            m21 = ((lhs.m20 * rhs.m01) + (lhs.m21 * rhs.m11)) + (lhs.m22 * rhs.m21),
            m22 = ((lhs.m20 * rhs.m02) + (lhs.m21 * rhs.m12)) + (lhs.m22 * rhs.m22)
        };
}

