namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class QuantumShopContent : DealItemContent
    {
        [SerializeField]
        private Button button;

        protected override void FillFromEntity(Entity entity)
        {
            string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
            base.banner.SpriteUid = spriteUid;
            Dictionary<int, int> packXPrice = entity.GetComponent<PackPriceComponent>().PackXPrice;
            base.title.text = string.Format(this.Title, packXPrice.Keys.First<int>());
            base.description.text = this.Description;
            base.price.text = string.Format(this.Price, packXPrice.Values.First<int>().ToString(), "<sprite=9>");
            base.FillFromEntity(entity);
        }

        public virtual string Title { get; set; }

        public virtual string Price { get; set; }

        public virtual string Description { get; set; }
    }
}

