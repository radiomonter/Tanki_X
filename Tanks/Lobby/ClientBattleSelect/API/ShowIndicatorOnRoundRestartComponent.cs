namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(LayoutElement)), RequireComponent(typeof(CanvasGroup))]
    public class ShowIndicatorOnRoundRestartComponent : MonoBehaviour, Component
    {
        public void Hide()
        {
            base.GetComponent<CanvasGroup>().alpha = 0f;
            base.GetComponent<LayoutElement>().ignoreLayout = true;
        }

        public void Show()
        {
            base.GetComponent<CanvasGroup>().alpha = 1f;
            base.GetComponent<LayoutElement>().ignoreLayout = false;
        }
    }
}

