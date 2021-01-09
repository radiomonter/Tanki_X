namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ModuleCardOutlineView : MonoBehaviour
    {
        [SerializeField]
        private Color[] tierColor;
        [SerializeField]
        private OutlineObject outline;
        [SerializeField]
        private ModuleCardView card;

        public void Start()
        {
            this.outline.GlowColor = this.tierColor[this.card.tierNumber];
        }
    }
}

