namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;

    public class TMPLocalize : MonoBehaviour
    {
        [SerializeField]
        private string uid;

        protected void Awake()
        {
            if (Application.isPlaying)
            {
                string str = LocalizationUtils.Localize(this.uid);
                TextMeshProUGUI component = base.GetComponent<TextMeshProUGUI>();
                if (!string.IsNullOrEmpty(str) && (component != null))
                {
                    component.text = str.Replace(@"\n", "\n");
                }
            }
        }

        public string TextUid =>
            this.uid;
    }
}

