namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using UnityEngine;

    public class GameObjectAlreadyContainsEntityBehaviour : Exception
    {
        public GameObjectAlreadyContainsEntityBehaviour(GameObject gameObject) : base($"GameObject {gameObject} already contains EntityBehaviour")
        {
        }
    }
}

