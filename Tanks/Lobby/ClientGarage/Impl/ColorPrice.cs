namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(AbstractPriceLabelComponent))]
    public class ColorPrice : ColorText
    {
        public override void SetColor(ColorData colorData)
        {
            base.GetComponent<AbstractPriceLabelComponent>().Color = colorData.Color;
            if (!base.noApplyMaterial)
            {
                base.text.material = colorData.material;
            }
        }
    }
}

