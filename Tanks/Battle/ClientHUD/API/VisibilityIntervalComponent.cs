namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class VisibilityIntervalComponent : MonoBehaviour, Component
    {
        public int intervalInSec = 2;
    }
}

