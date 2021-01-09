namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class VisibilityPeriodsComponent : MonoBehaviour, Component
    {
        public int firstIntervalInSec = 30;
        public int lastIntervalInSec = 30;
        public int spaceIntervalInSec = 5;
        public int lastBlinkingIntervalInSec = 10;
    }
}

