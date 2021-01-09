namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TransitionCameraComponent : Component
    {
        public TransitionCameraComponent()
        {
            this.P1 = Vector3.zero;
            this.P2 = Vector3.zero;
            this.P3 = Vector3.zero;
            this.P4 = Vector3.zero;
            this.Speed = 0f;
            this.Distance = 0f;
        }

        public void Reset()
        {
            this.P1 = Vector3.zero;
            this.P2 = Vector3.zero;
            this.P3 = Vector3.zero;
            this.P4 = Vector3.zero;
            this.Speed = 0f;
            this.Distance = 0f;
            this.Spawn = false;
            this.CameraSaveData = null;
            this.TransitionComplete = false;
            this.TotalDistance = 0f;
            this.Acceleration = 0f;
            this.AngleValuesX = null;
            this.AngleValuesY = null;
        }

        public bool Spawn { get; set; }

        public Tanks.Battle.ClientGraphics.Impl.CameraSaveData CameraSaveData { get; set; }

        public Vector3 P1 { get; set; }

        public Vector3 P2 { get; set; }

        public Vector3 P3 { get; set; }

        public Vector3 P4 { get; set; }

        public AngleValues AngleValuesX { get; set; }

        public AngleValues AngleValuesY { get; set; }

        public float TotalDistance { get; set; }

        public float Acceleration { get; set; }

        public float Speed { get; set; }

        public float Distance { get; set; }

        public bool TransitionComplete { get; set; }
    }
}

