namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class GraffitiVisualEffect : MonoBehaviour
    {
        public ImageSkin Image;
        [SerializeField]
        private GameObject _rareEffect;
        [SerializeField]
        private GameObject _epicEffect;
        [SerializeField]
        private GameObject _legendaryEffect;

        private void OnDisable()
        {
            this._rareEffect.SetActive(false);
            this._epicEffect.SetActive(false);
            this._legendaryEffect.SetActive(false);
        }

        public ItemRarityType Rarity
        {
            set
            {
                if (value == ItemRarityType.RARE)
                {
                    this._rareEffect.SetActive(true);
                }
                else if (value == ItemRarityType.EPIC)
                {
                    this._epicEffect.SetActive((bool) base.transform);
                }
                else if (value == ItemRarityType.LEGENDARY)
                {
                    this._legendaryEffect.SetActive(true);
                }
            }
        }
    }
}

