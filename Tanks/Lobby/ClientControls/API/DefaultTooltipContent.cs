namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;

    public class DefaultTooltipContent : MonoBehaviour, ITooltipContent
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public void Init(object data)
        {
            string text = (data as string).Replace(@"\n", "\n");
            this.text.SetText(text);
        }
    }
}

