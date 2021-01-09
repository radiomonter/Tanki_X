namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InputActionContainer
    {
        [SerializeField]
        public InputActionContextId contextId;
        [SerializeField]
        public InputActionId actionId;
    }
}

