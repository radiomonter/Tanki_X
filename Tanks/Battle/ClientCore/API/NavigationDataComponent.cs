namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class NavigationDataComponent : Component
    {
        public Vector3 MovePosition { get; set; }

        public bool ObstacleOnCriticalDistance { get; set; }

        public bool TankInTheFront { get; set; }

        public bool ObstacleOnAvoidanceDistance { get; set; }

        public float LastMove { get; set; }

        public float LastTurn { get; set; }

        public BehaviourTreeNode BehavouTree { get; set; }

        public Tanks.Battle.ClientCore.API.PathData PathData { get; set; }
    }
}

