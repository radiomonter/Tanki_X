namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class FractionButtonComponent : BehaviourComponent
    {
        public FractionActions Action;
        [HideInInspector]
        public Entity FractionEntity;

        public enum FractionActions
        {
            SELECT,
            AWARDS,
            LEARN_MORE
        }
    }
}

