namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct SelfTargetHitEffectHUDData
    {
        private Vector3 enemyWeaponWorldSpace;
        private Vector2 boundsPosition;
        private Vector3 upwardsNRM;
        private Vector2 boundsPosCanvas;
        private Vector2 cnvSize;
        private float length;
        public SelfTargetHitEffectHUDData(Vector3 enemyWeaponWorldSpace, Vector2 boundsPosition, Vector2 boundsPosCanvas, Vector3 upwardsNRM, Vector2 cnvSize, float length)
        {
            this.enemyWeaponWorldSpace = enemyWeaponWorldSpace;
            this.boundsPosition = boundsPosition;
            this.boundsPosCanvas = boundsPosCanvas;
            this.cnvSize = cnvSize;
            this.upwardsNRM = upwardsNRM;
            this.length = length;
        }

        public Vector2 BoundsPosition =>
            this.boundsPosition;
        public Vector3 UpwardsNrm =>
            this.upwardsNRM;
        public float Length =>
            this.length;
        public Vector2 BoundsPosCanvas =>
            this.boundsPosCanvas;
        public Vector3 EnemyWeaponWorldSpace =>
            this.enemyWeaponWorldSpace;
        public Vector2 CnvSize =>
            this.cnvSize;
    }
}

