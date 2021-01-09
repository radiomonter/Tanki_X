namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class VisualProperty
    {
        public VisualProperty()
        {
            this.Format = "0";
            this.Unit = string.Empty;
        }

        public float GetAdditionalValue(float coef) => 
            this.InitialAdditionValue + ((this.FinalAdditionValue - this.InitialAdditionValue) * coef);

        public string GetFormatedValue(float coef)
        {
            string str = this.GetValue(coef).ToString(this.Format);
            if (this.InitialAdditionValue >= this.InitialValue)
            {
                str = str + " - " + this.GetAdditionalValue(coef).ToString(this.Format);
            }
            return str;
        }

        public float GetProgress(int level)
        {
            float b = this.InitialValue + Mathf.Abs((float) (this.FinalValue - this.InitialValue));
            return (Mathf.Lerp(this.InitialValue, b, (float) (level / UpgradablePropertiesUtils.MAX_LEVEL)) / b);
        }

        public float GetValue(float coef) => 
            this.InitialValue + ((this.FinalValue - this.InitialValue) * coef);

        public string Name { get; set; }

        public float InitialValue { get; set; }

        public float FinalValue { get; set; }

        public float InitialAdditionValue { get; set; }

        public float FinalAdditionValue { get; set; }

        public string Format { get; set; }

        public string Unit { get; set; }
    }
}

