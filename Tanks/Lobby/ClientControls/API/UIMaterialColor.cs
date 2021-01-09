namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIMaterialColor : MonoBehaviour
    {
        private Graphic graphic;
        private Material material;

        private void Awake()
        {
            this.graphic = base.GetComponent<Graphic>();
            this.material = new Material(this.graphic.material);
            this.graphic.material = this.material;
        }

        private void Update()
        {
            if (this.material.color != this.graphic.color)
            {
                this.material.SetColor("_Color", this.graphic.color);
            }
        }
    }
}

