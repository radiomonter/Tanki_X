namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class PaletteColor : MonoBehaviour
    {
        [SerializeField]
        private Palette palette;
        [SerializeField]
        private int uid;
        [SerializeField]
        private bool applyToChildren;
        private List<Graphic> graphicCache = new List<Graphic>();

        private void Apply(Palette palette)
        {
            if (!this.applyToChildren)
            {
                Graphic component = base.GetComponent<Graphic>();
                this.ApplyToGraphic(component, palette);
            }
            else
            {
                base.GetComponentsInChildren<Graphic>(this.graphicCache);
                foreach (Graphic graphic in this.graphicCache)
                {
                    this.ApplyToGraphic(graphic, palette);
                }
            }
        }

        private void ApplyToGraphic(Graphic graphic, Palette palette)
        {
            graphic.color = palette.Apply(this.uid, graphic.color);
        }

        private void Start()
        {
            this.Apply(this.palette);
            if (Application.isPlaying)
            {
                Destroy(this);
            }
        }
    }
}

