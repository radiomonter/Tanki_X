namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Graphic))]
    public class CopyColor : MonoBehaviour
    {
        [SerializeField]
        private List<Graphic> targets;
        private Graphic source;

        private void Awake()
        {
            this.source = base.GetComponent<Graphic>();
        }

        private void Update()
        {
            foreach (Graphic graphic in this.targets)
            {
                graphic.color = this.source.color;
            }
        }
    }
}

