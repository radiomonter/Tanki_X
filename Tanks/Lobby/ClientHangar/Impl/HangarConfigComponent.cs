namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class HangarConfigComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float autoRotateSpeed = 15f;
        [SerializeField]
        private float keyboardRotateSpeed = 30f;
        [SerializeField]
        private float mouseRotateFactor = 0.7f;
        [SerializeField]
        private float decelerationRotateFactor = 1.5f;
        [SerializeField]
        private float autoRotateDelay = 30f;
        [SerializeField]
        private float flightToLocationTime = 1f;
        [SerializeField]
        private float flightToLocationHigh = 5f;
        [SerializeField]
        private float flightToTankTime = 1f;
        [SerializeField]
        private float flightToTankRadius = 2f;

        public float AutoRotateSpeed =>
            this.autoRotateSpeed;

        public float KeyboardRotateSpeed =>
            this.keyboardRotateSpeed;

        public float MouseRotateFactor =>
            this.mouseRotateFactor;

        public float DecelerationRotateFactor =>
            this.decelerationRotateFactor;

        public float AutoRotateDelay =>
            this.autoRotateDelay;

        public float FlightToLocationTime =>
            this.flightToLocationTime;

        public float FlightToTankTime =>
            this.flightToTankTime;

        public float FlightToTankRadius =>
            this.flightToTankRadius;

        public float FlightToLocationHigh =>
            this.flightToLocationHigh;
    }
}

