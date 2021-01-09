namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class AbstractPriceLabelSystem : ECSSystem
    {
        private string GetPriceString(long priceValue)
        {
            string str = priceValue.ToString();
            if (priceValue > 0x98967fL)
            {
                str = this.RoundPrice(priceValue);
            }
            return str;
        }

        private string RoundPrice(long price)
        {
            int num = 0;
            string str = string.Empty;
            while (price >= 0x3e8L)
            {
                price /= 0x3e8L;
                num++;
                str = str + "K";
            }
            return (price + str);
        }

        protected void UpdatePriceForUser(long priceValue, long oldPriceValue, AbstractPriceLabelComponent priceLabel, long userMoney)
        {
            priceLabel.Text.color = (userMoney >= priceValue) ? priceLabel.DefaultColor : priceLabel.shortageColor;
            priceLabel.Text.text = this.GetPriceString(priceValue);
            priceLabel.Price = priceValue;
            priceLabel.OldPriceVisible = (oldPriceValue > 0L) && (oldPriceValue != priceValue);
            priceLabel.OldPrice = oldPriceValue;
            priceLabel.OldPriceText = this.GetPriceString(oldPriceValue);
        }
    }
}

