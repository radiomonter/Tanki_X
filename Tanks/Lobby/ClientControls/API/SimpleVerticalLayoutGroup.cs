namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Layout/Simple Vertical Layout Group", 0x99)]
    public class SimpleVerticalLayoutGroup : SimpleLayoutGroup
    {
        protected SimpleVerticalLayoutGroup()
        {
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            base.CalcAlongAxis(0, true);
        }

        public override void CalculateLayoutInputVertical()
        {
            base.CalcAlongAxis(1, true);
        }

        public override void SetLayoutHorizontal()
        {
            base.SetChildrenAlongAxis(0, true);
        }

        public override void SetLayoutVertical()
        {
            base.SetChildrenAlongAxis(1, true);
        }
    }
}

