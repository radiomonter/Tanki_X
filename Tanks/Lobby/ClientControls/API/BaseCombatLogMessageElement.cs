namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public abstract class BaseCombatLogMessageElement : MonoBehaviour
    {
        [SerializeField]
        protected UnityEngine.RectTransform rectTransform;

        protected BaseCombatLogMessageElement()
        {
        }

        public UnityEngine.RectTransform RectTransform =>
            this.rectTransform;
    }
}

