namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    public class NormalizedAnimatedValue : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float value;
    }
}

