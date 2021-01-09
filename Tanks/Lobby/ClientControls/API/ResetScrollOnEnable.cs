namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(ScrollRect))]
    public class ResetScrollOnEnable : MonoBehaviour
    {
        private void OnEnable()
        {
            base.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        }

        private void Start()
        {
            base.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        }
    }
}

