﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ChassisAnimationComponent : MonoBehaviour, Component
    {
        public string trackPointName = "_wh_trk";
        public string movingWheelName = "_wheel_";
        public string rotatingWheelName = "_fixed";
        public string wheelsMeshName = "body";
        public float verticalOffset;
        public float horizontalOffset;
        public TrackBindingData leftTrackData = new TrackBindingData();
        public TrackBindingData rightTrackData = new TrackBindingData();
        public float highDistance = 1f;
        public float lowDistance = 1f;
        public float maxStretchingCoeff = 1.04f;
        public bool additionalRaycastsEnabled = true;
        public float tracksMaterialSpeedMultiplyer = 1f;
        public float smoothSpeed = 1f;

        public Material TracksMaterial { get; set; }

        public float LeftTrackPosition { get; set; }

        public float RightTrackPosition { get; set; }
    }
}

