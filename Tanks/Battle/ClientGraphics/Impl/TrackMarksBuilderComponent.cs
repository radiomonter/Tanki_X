namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class TrackMarksBuilderComponent : Component
    {
        public Transform[] leftWheels;
        public Transform[] rightWheels;
        public WheelData[] prevLeftWheelsData;
        public WheelData[] prevRightWheelsData;
        public WheelData[] currentLeftWheelsData;
        public WheelData[] currentRightWheelsData;
        public WheelData[] tempLeftWheelsData;
        public WheelData[] tempRightWheelsData;
        public Vector3[] positions;
        public Vector3[] nextPositions;
        public Vector3[] normals;
        public Vector3[] nextNormals;
        public Vector3[] directions;
        public bool[] contiguous;
        public bool[] prevHits;
        public float[] remaingDistance;
        public bool[] resetWheels;
        public float[] side;
        public float moveStep;
        public Rigidbody rigidbody;
        private Camera _cachedCamera;

        public Camera CachedCamera
        {
            get
            {
                if (!this._cachedCamera)
                {
                    this._cachedCamera = Camera.main;
                }
                return this._cachedCamera;
            }
        }
    }
}

