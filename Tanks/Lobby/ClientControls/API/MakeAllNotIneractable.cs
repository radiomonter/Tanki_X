namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MakeAllNotIneractable : MonoBehaviour
    {
        [ContextMenu("Make not interactable")]
        public void MakeNotInteractable()
        {
            foreach (Selectable selectable in base.GetComponentsInChildren<Selectable>(true))
            {
                selectable.interactable = false;
            }
            foreach (Image image in base.GetComponentsInChildren<Image>(true))
            {
                image.raycastTarget = false;
            }
            foreach (Text text in base.GetComponentsInChildren<Text>(true))
            {
                text.raycastTarget = false;
            }
            foreach (TextMeshProUGUI ougui in base.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                ougui.raycastTarget = false;
            }
        }
    }
}

