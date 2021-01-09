namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GraffitiAntiSpamTimerComponent : MonoBehaviour, Component
    {
        public Dictionary<string, GraffityInfo> GraffitiDelayDictionary = new Dictionary<string, GraffityInfo>();

        public float SprayDelay { get; set; }

        public class GraffityInfo
        {
            public CreateGraffitiEvent CreateGraffitiEvent;
            public float Time;
        }
    }
}

