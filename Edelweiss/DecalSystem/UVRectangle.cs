namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    [Serializable]
    public class UVRectangle
    {
        public string name;
        public Vector2 lowerLeftUV;
        public Vector2 upperRightUV;

        public UVRectangle()
        {
            this.name = "UVRectangle";
            this.lowerLeftUV = Vector2.zero;
            this.upperRightUV = Vector3.one;
            this.name = "UVRectangle";
            this.lowerLeftUV = Vector2.zero;
            this.upperRightUV = Vector2.one;
        }

        public UVRectangle(UVRectangle a_Other)
        {
            this.name = "UVRectangle";
            this.lowerLeftUV = Vector2.zero;
            this.upperRightUV = Vector3.one;
            this.name = string.Copy(a_Other.name);
            this.lowerLeftUV = a_Other.lowerLeftUV;
            this.upperRightUV = a_Other.upperRightUV;
        }

        public override string ToString() => 
            this.name;

        public Vector2 Size =>
            this.upperRightUV - this.lowerLeftUV;
    }
}

