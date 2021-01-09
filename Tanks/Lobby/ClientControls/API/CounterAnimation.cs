namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    public class CounterAnimation : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float value;
        private int targetValue;
        private Text text;

        private void OnEnable()
        {
            this.text = base.GetComponent<Text>();
            this.targetValue = int.Parse(this.text.text);
        }

        private void Update()
        {
            this.text.text = ((int) (this.value * this.targetValue)).ToStringSeparatedByThousands();
        }
    }
}

