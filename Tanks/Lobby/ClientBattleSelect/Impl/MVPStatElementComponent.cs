namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class MVPStatElementComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI count;
        [SerializeField]
        private GameObject best;
        private int _count;

        public void Hide()
        {
            base.gameObject.SetActive(false);
        }

        public void SetBest(bool isBest)
        {
            this.best.gameObject.SetActive(isBest);
        }

        public bool ShowIfCan()
        {
            base.gameObject.SetActive(this._count > 0);
            return (this._count > 0);
        }

        public int Count
        {
            get => 
                this._count;
            set
            {
                this._count = value;
                this.count.text = this._count.ToString();
            }
        }
    }
}

