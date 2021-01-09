namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BattleSelectScreenLayout : UIBehaviour, ILayoutSelfController, ILayoutController
    {
        public RectTransform scoreTableParent;

        public void SetLayoutHorizontal()
        {
        }

        public void SetLayoutVertical()
        {
            if (this.scoreTableParent.childCount > 0)
            {
                RectTransform component = base.GetComponent<RectTransform>();
                float height = ((RectTransform) this.scoreTableParent.GetChild(0)).rect.height;
                if (component.sizeDelta.y != height)
                {
                    component.sizeDelta = new Vector2(component.sizeDelta.x, height);
                }
            }
        }
    }
}

