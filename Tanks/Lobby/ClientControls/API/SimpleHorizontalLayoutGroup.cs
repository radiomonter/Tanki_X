namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Layout/Simple Horizontal Layout Group", 0x98)]
    public class SimpleHorizontalLayoutGroup : SimpleLayoutGroup
    {
        protected SimpleHorizontalLayoutGroup()
        {
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            base.CalcAlongAxis(0, false);
        }

        public override void CalculateLayoutInputVertical()
        {
            base.CalcAlongAxis(1, false);
        }

        public override void SetLayoutHorizontal()
        {
            base.SetChildrenAlongAxis(0, false);
        }

        public override void SetLayoutVertical()
        {
            base.SetChildrenAlongAxis(1, false);
        }
    }
}

