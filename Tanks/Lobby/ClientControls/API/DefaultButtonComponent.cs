﻿namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class DefaultButtonComponent : LocalizedControl, Component
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public virtual string Text
        {
            set => 
                this.text.text = value.ToUpper();
        }
    }
}

