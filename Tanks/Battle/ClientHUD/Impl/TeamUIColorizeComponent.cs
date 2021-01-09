namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class TeamUIColorizeComponent : BehaviourComponent
    {
        [SerializeField]
        private List<Color> redColors;
        [SerializeField]
        private List<Color> blueColors;
        [SerializeField]
        private List<Graphic> elements;

        private void Colorize(List<Color> colors)
        {
            for (int i = 0; i < this.elements.Count; i++)
            {
                this.elements[i].color = colors[i];
            }
        }

        public void ColorizeBlue()
        {
            this.Colorize(this.blueColors);
        }

        public void ColorizeRed()
        {
            this.Colorize(this.redColors);
        }
    }
}

