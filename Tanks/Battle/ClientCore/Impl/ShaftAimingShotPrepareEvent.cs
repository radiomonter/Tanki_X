namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShaftAimingShotPrepareEvent : BaseShotPrepareEvent
    {
        public ShaftAimingShotPrepareEvent()
        {
        }

        public ShaftAimingShotPrepareEvent(Vector3 workingDir)
        {
            this.WorkingDir = workingDir;
        }

        public Vector3 WorkingDir { get; set; }
    }
}

