namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;

    public class WithHeaderTooltipContent : MonoBehaviour, ITooltipContent
    {
        [SerializeField]
        private TextMeshProUGUI header;
        [SerializeField]
        private TextMeshProUGUI text;

        public void Init(object data)
        {
            string[] strArray = data as string[];
            this.header.text = strArray[0].Replace(@"\n", "\n");
            this.text.text = strArray[1].Replace(@"\n", "\n");
        }
    }
}

