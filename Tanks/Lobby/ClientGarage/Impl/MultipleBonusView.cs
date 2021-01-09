namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class MultipleBonusView : MonoBehaviour
    {
        public TextMeshProUGUI countText;
        public TextMeshProUGUI text;
        public GameObject back;

        public void UpdateView(long amount)
        {
            this.countText.text = "x" + amount;
            this.back.SetActive(true);
            this.text.text = this.text.text.ToUpper();
        }
    }
}

