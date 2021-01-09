namespace Tanks.Battle.ClientCore.API
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public abstract class BaseShotEvent : TimeValidateEvent
    {
        public BaseShotEvent()
        {
        }

        public BaseShotEvent(Vector3 shotDirection)
        {
            this.ShotDirection = shotDirection;
        }

        [ProtocolOptional]
        public Vector3 ShotDirection { get; set; }

        public int ShotId { get; set; }
    }
}

