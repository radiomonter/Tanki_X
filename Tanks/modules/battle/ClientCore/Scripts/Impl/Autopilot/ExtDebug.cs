namespace tanks.modules.battle.ClientCore.Scripts.Impl.Autopilot
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class ExtDebug
    {
        private static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance) => 
            origin + (direction.normalized * hitInfoDistance);

        public static void DrawBox(Box box, Color color)
        {
            Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
            Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
            Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
            Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);
            Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
            Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
            Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
            Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);
            Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
            Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
            Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
            Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
        {
            DrawBox(new Box(origin, halfExtents, orientation), color);
        }

        public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
        {
            direction.Normalize();
            Box box = new Box(origin, halfExtents, orientation);
            Box box2 = new Box(origin + (direction * distance), halfExtents, orientation);
            Debug.DrawLine(box.backBottomLeft, box2.backBottomLeft, color);
            Debug.DrawLine(box.backBottomRight, box2.backBottomRight, color);
            Debug.DrawLine(box.backTopLeft, box2.backTopLeft, color);
            Debug.DrawLine(box.backTopRight, box2.backTopRight, color);
            Debug.DrawLine(box.frontTopLeft, box2.frontTopLeft, color);
            Debug.DrawLine(box.frontTopRight, box2.frontTopRight, color);
            Debug.DrawLine(box.frontBottomLeft, box2.frontBottomLeft, color);
            Debug.DrawLine(box.frontBottomRight, box2.frontBottomRight, color);
            DrawBox(box, color);
            DrawBox(box2, color);
        }

        public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
        {
            origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
            DrawBox(origin, halfExtents, orientation, color);
        }

        private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 vector = point - pivot;
            return (pivot + (rotation * vector));
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        public struct Box
        {
            public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
            {
                this.Rotate(orientation);
            }

            public Box(Vector3 origin, Vector3 halfExtents)
            {
                this = new ExtDebug.Box();
                this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
                this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
                this.origin = origin;
            }

            public Vector3 localFrontTopLeft { get; private set; }
            public Vector3 localFrontTopRight { get; private set; }
            public Vector3 localFrontBottomLeft { get; private set; }
            public Vector3 localFrontBottomRight { get; private set; }
            public Vector3 localBackTopLeft =>
                -this.localFrontBottomRight;
            public Vector3 localBackTopRight =>
                -this.localFrontBottomLeft;
            public Vector3 localBackBottomLeft =>
                -this.localFrontTopRight;
            public Vector3 localBackBottomRight =>
                -this.localFrontTopLeft;
            public Vector3 frontTopLeft =>
                this.localFrontTopLeft + this.origin;
            public Vector3 frontTopRight =>
                this.localFrontTopRight + this.origin;
            public Vector3 frontBottomLeft =>
                this.localFrontBottomLeft + this.origin;
            public Vector3 frontBottomRight =>
                this.localFrontBottomRight + this.origin;
            public Vector3 backTopLeft =>
                this.localBackTopLeft + this.origin;
            public Vector3 backTopRight =>
                this.localBackTopRight + this.origin;
            public Vector3 backBottomLeft =>
                this.localBackBottomLeft + this.origin;
            public Vector3 backBottomRight =>
                this.localBackBottomRight + this.origin;
            public Vector3 origin { get; private set; }
            public void Rotate(Quaternion orientation)
            {
                this.localFrontTopLeft = ExtDebug.RotatePointAroundPivot(this.localFrontTopLeft, Vector3.zero, orientation);
                this.localFrontTopRight = ExtDebug.RotatePointAroundPivot(this.localFrontTopRight, Vector3.zero, orientation);
                this.localFrontBottomLeft = ExtDebug.RotatePointAroundPivot(this.localFrontBottomLeft, Vector3.zero, orientation);
                this.localFrontBottomRight = ExtDebug.RotatePointAroundPivot(this.localFrontBottomRight, Vector3.zero, orientation);
            }
        }
    }
}

