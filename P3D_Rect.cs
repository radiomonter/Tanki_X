using System;
using System.Runtime.InteropServices;

[Serializable, StructLayout(LayoutKind.Sequential)]
public struct P3D_Rect
{
    public int XMin;
    public int XMax;
    public int YMin;
    public int YMax;
    public P3D_Rect(int newXMin, int newXMax, int newYMin, int newYMax)
    {
        this.XMin = newXMin;
        this.XMax = newXMax;
        this.YMin = newYMin;
        this.YMax = newYMax;
    }

    public bool IsSet =>
        (this.XMin != this.XMax) && (this.YMin != this.YMax);
    public int Width =>
        this.XMax - this.XMin;
    public int Height =>
        this.YMax - this.YMin;
    public void Set(int newXMin, int newXMax, int newYMin, int newYMax)
    {
        this.XMin = newXMin;
        this.XMax = newXMax;
        this.YMin = newYMin;
        this.YMax = newYMax;
    }

    public void Add(int newX, int newY)
    {
        this.Add(newX, newX + 1, newY, newY + 1);
    }

    public void Add(P3D_Rect rect)
    {
        this.Add(rect.XMin, rect.XMax, rect.YMin, rect.YMax);
    }

    public void Add(int newXMin, int newXMax, int newYMin, int newYMax)
    {
        if (this.Width == 0)
        {
            this.XMin = newXMin;
            this.XMax = newXMax;
        }
        else
        {
            if (newXMin < this.XMin)
            {
                this.XMin = newXMin;
            }
            if (newXMax > this.XMax)
            {
                this.XMax = newXMax;
            }
        }
        if (this.Height == 0)
        {
            this.YMin = newYMin;
            this.YMax = newYMax;
        }
        else
        {
            if (newYMin < this.YMin)
            {
                this.YMin = newYMin;
            }
            if (newYMax > this.YMax)
            {
                this.YMax = newYMax;
            }
        }
    }

    public bool Overlaps(P3D_Rect other) => 
        this.IsSet && (other.IsSet && ((this.XMax > other.XMin) ? ((this.XMin < other.XMax) ? ((this.YMax > other.YMin) ? (this.YMin < other.YMax) : false) : false) : false));

    public void Clear()
    {
        this.XMin = 0;
        this.XMax = 0;
        this.YMin = 0;
        this.YMax = 0;
    }
}

