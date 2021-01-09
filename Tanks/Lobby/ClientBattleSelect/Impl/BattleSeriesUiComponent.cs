namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class BattleSeriesUiComponent : BehaviourComponent
    {
        [Header("Colors"), SerializeField]
        private Color _battleSeriesColor;
        [SerializeField]
        private Color _deserterColor;
        [SerializeField]
        private Color _defaultColor;
        [Header("Texts"), SerializeField]
        private LocalizedField _battlesAmountSingularText;
        [SerializeField]
        private LocalizedField _battlesAmountPlural1Text;
        [SerializeField]
        private LocalizedField _battlesAmountPlural2Text;
        [SerializeField]
        private TextMeshProUGUI _battleSeriesText;
        [SerializeField]
        private LocalizedField _battleSeriesString;
        [SerializeField]
        private LocalizedField _deserterString;
        [Space(10f), SerializeField]
        private TextMeshProUGUI _additionalExpText;
        [SerializeField]
        private LocalizedField _additionalExpString;
        [SerializeField]
        private TextMeshProUGUI _additionScoresText;
        [SerializeField]
        private LocalizedField _additionScoresString;
        [SerializeField]
        private TextMeshProUGUI _additionalMessageText;
        [SerializeField]
        private LocalizedField _nextBattleNotificationString;
        [SerializeField]
        private LocalizedField _maxSeriesAchiviedString;
        [SerializeField]
        private LocalizedField _deserterAdditionalMessageString;
        [Header("Main Icon"), SerializeField]
        private TextMeshProUGUI _battleSeriesCountText;
        [SerializeField]
        private ImageSkin _battleSeriesImage;
        [SerializeField]
        private string[] _battleImageUids;
        [SerializeField]
        private string _deserterImageUid;

        private string Pluralize(int amount)
        {
            CaseType @case = CasesUtil.GetCase(amount);
            if (@case == CaseType.DEFAULT)
            {
                return string.Format(this._battlesAmountPlural1Text.Value, amount);
            }
            if (@case == CaseType.ONE)
            {
                return string.Format(this._battlesAmountSingularText.Value, amount);
            }
            if (@case != CaseType.TWO)
            {
                throw new Exception("ivnalid case");
            }
            return string.Format(this._battlesAmountPlural2Text.Value, amount);
        }

        public int CurrentBattleCount
        {
            set
            {
                if (value <= 0)
                {
                    this._battleSeriesText.color = this._defaultColor;
                    this._battleSeriesText.text = string.Format((string) this._deserterString, this.Pluralize(Mathf.Abs(value)));
                }
                else
                {
                    this._battleSeriesText.color = this._battleSeriesColor;
                    this._battleSeriesText.text = string.Format((string) this._battleSeriesString, value);
                    this._battleSeriesCountText.color = this._battleSeriesColor;
                    string str = ArabianToRomanNumConverter.ArabianToRoman(value);
                    this._battleSeriesCountText.text = str;
                }
            }
        }

        public float BattleSeriesPercent
        {
            set
            {
                if (value <= 0f)
                {
                    this._additionalMessageText.text = (string) this._deserterAdditionalMessageString;
                    this._battleSeriesCountText.gameObject.SetActive(false);
                    this._battleSeriesImage.SpriteUid = this._deserterImageUid;
                    this._battleSeriesImage.GetComponent<Image>().color = this._deserterColor;
                }
                else
                {
                    bool flag = value >= 1f;
                    this._battleSeriesCountText.gameObject.SetActive(!flag);
                    this._additionalMessageText.text = !flag ? ((string) this._nextBattleNotificationString) : ((string) this._maxSeriesAchiviedString);
                    this._battleSeriesImage.SpriteUid = !flag ? this._battleImageUids[(int) Mathf.Floor((this._battleImageUids.Length - 1) * value)] : this._battleImageUids[this._battleImageUids.Length - 1];
                    this._battleSeriesImage.GetComponent<Image>().color = this._defaultColor;
                }
            }
        }

        public float ExperienceMultiplier
        {
            set
            {
                if (value <= 1f)
                {
                    this._additionalExpText.gameObject.SetActive(false);
                }
                else
                {
                    this._additionalExpText.gameObject.SetActive(true);
                    string str = $"{(value * 100f) - 100f:0}";
                    string[] textArray1 = new string[] { "<color=#", this._battleSeriesColor.ToHexString(), ">+", str, "%</color>" };
                    this._additionalExpText.text = string.Format((string) this._additionalExpString, string.Concat(textArray1));
                }
            }
        }

        public float ContainerScoreMultiplier
        {
            set
            {
                if (value <= 1f)
                {
                    this._additionScoresText.gameObject.SetActive(false);
                }
                else
                {
                    this._additionScoresText.gameObject.SetActive(true);
                    string str = $"{(value * 100f) - 100f:0}";
                    string[] textArray1 = new string[] { "<color=#", this._battleSeriesColor.ToHexString(), ">+", str, "%</color>" };
                    this._additionScoresText.text = string.Format((string) this._additionScoresString, string.Concat(textArray1));
                }
            }
        }
    }
}

