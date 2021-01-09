namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientPayment.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class SynthUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TMP_InputField crystals;
        [SerializeField]
        private TMP_InputField xCrystals;
        [SerializeField]
        private long defaultXCrystalsAmount = 100;
        [SerializeField]
        private Animator synthButtonAnimator;
        [SerializeField]
        private ExchangeConfirmationWindow exchangeConfirmation;
        private bool calculating;

        private void Awake()
        {
            this.crystals.onValueChanged.AddListener(new UnityAction<string>(this.CalculateXCrystals));
            this.crystals.onEndEdit.AddListener(new UnityAction<string>(this.RoundCrystals));
            this.xCrystals.onValueChanged.AddListener(new UnityAction<string>(this.CalculateCrystals));
        }

        private void CalculateCrystals(string value)
        {
            if (!this.calculating)
            {
                this.calculating = true;
                long result = 0L;
                long.TryParse(value, out result);
                long crystals = (long) (result * ExchangeRateComponent.ExhchageRate);
                this.ValidateButton(crystals, result);
                this.crystals.text = crystals.ToString();
                this.calculating = false;
            }
        }

        private void CalculateXCrystals(string value)
        {
            if (!this.calculating)
            {
                this.calculating = true;
                long result = 0L;
                long.TryParse(value, out result);
                long xCrystals = (long) (((float) result) / ExchangeRateComponent.ExhchageRate);
                this.ValidateButton(result, xCrystals);
                this.xCrystals.text = xCrystals.ToString();
                this.calculating = false;
            }
        }

        private void OnDisable()
        {
            this.crystals.text = string.Empty;
            this.xCrystals.text = string.Empty;
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(this.xCrystals.text))
            {
                this.xCrystals.text = this.defaultXCrystalsAmount.ToString();
                this.CalculateCrystals(this.xCrystals.text);
            }
        }

        public void OnSynth()
        {
            long xCrystals = long.Parse(this.xCrystals.text);
            this.exchangeConfirmation.Show(SelfUserComponent.SelfUser, xCrystals, (long) (ExchangeRateComponent.ExhchageRate * xCrystals));
        }

        public void OnXCrystalsChanged()
        {
            this.CalculateCrystals(this.xCrystals.text);
        }

        private void RoundCrystals(string value)
        {
            this.CalculateCrystals(this.xCrystals.text);
        }

        public void SetCrystals(long cry)
        {
            this.crystals.text = cry.ToString();
            this.CalculateXCrystals(this.crystals.text);
        }

        public void SetXCrystals(long xcry)
        {
            this.xCrystals.text = xcry.ToString();
            this.CalculateCrystals(this.xCrystals.text);
        }

        private void Start()
        {
            this.ValidateButton(long.Parse(this.crystals.text), long.Parse(this.xCrystals.text));
        }

        private void ValidateButton(long crystals, long xCrystals)
        {
            if (base.gameObject.activeInHierarchy)
            {
                this.synthButtonAnimator.SetBool("Visible", ((crystals > 0L) && (xCrystals > 0L)) && (xCrystals <= SelfUserComponent.SelfUser.GetComponent<UserXCrystalsComponent>().Money));
            }
        }
    }
}

