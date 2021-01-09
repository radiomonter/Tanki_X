namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public static class VulcanPhysicsUtils
    {
        public static void ApplyVulcanForce(Vector3 force, Rigidbody body, Vector3 pos, TankFallingComponent tankFalling, TrackComponent tracks)
        {
            body.AddForceAtPositionSafe(force, pos);
            if (!tankFalling.IsGrounded && ((tracks.LeftTrack.numContacts + tracks.RightTrack.numContacts) <= 0))
            {
                body.AddForceSafe(-force);
            }
        }
    }
}

