namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(UnityEngine.Animator))]
    public class ItemButtonComponent : BehaviourComponent
    {
        private UnityEngine.Animator animator;
        [SerializeField]
        private CooldownAnimation cooldown;
        [SerializeField]
        private PaletteColorField epicColor;
        [SerializeField]
        private PaletteColorField exceptionalColor;
        [SerializeField]
        private ImageSkin icon;
        [SerializeField]
        private TextMeshProUGUI keyBind;
        [SerializeField]
        private UnityEngine.Animator lockByEMPAnimator;
        [SerializeField]
        private Color CDColor;
        [SerializeField]
        private Color FullCDColor = new Color(1f, 0.15f, 0f, 0.74f);
        [SerializeField]
        private Color LowCDColor = new Color(0.3f, 0.9f, 0.5f, 0.74f);
        [SerializeField]
        private Color RageCDColor;
        [SerializeField]
        private Image CDFill;
        [SerializeField]
        private GameObject barRoot;
        [SerializeField]
        private AmmunitionBar ammunitionBarPrefab;
        [SerializeField]
        private AmmunitionBar[] ammunitionBars;
        private int maxItemAmmunitionCount;
        private int currentAmmunitionItemIndex;
        public bool ammunitionCountWasIncreased;
        private bool inCooldown;
        private float cooldownTime;
        private float cooldownTimer;
        private float _cooldownMultiplier;
        public bool isRage;
        public float CooldownCoeff = 1f;

        public void Activate()
        {
            this.Animator.SetTrigger("Activate");
        }

        public void ChangeCooldown(float time, float coeff, bool slotEnabled)
        {
            this.SetCooldownCoeff(coeff);
            if (this.isRage)
            {
                this._cooldownMultiplier = 1f / this.cooldown.Cooldown;
                this.CDFill.color = this.CDColor;
            }
            else
            {
                this._cooldownMultiplier = 1f / (this.cooldown.Cooldown / this.CooldownCoeff);
                this.CDFill.color = this.RageCDColor;
            }
            this.Animator.SetFloat("CooldownMultiplier", this._cooldownMultiplier);
        }

        public void CutCooldown(float cutTime)
        {
            this.cooldown.Cooldown = cutTime;
            this.Animator.SetTrigger("Bloodlust");
            this.Animator.SetFloat("CooldownMultiplier", 1f / cutTime);
        }

        public void Disable()
        {
            this.Animator.SetBool("Enabled", false);
        }

        public void Enable()
        {
            this.Animator.SetBool("Enabled", true);
        }

        public void FinishCooldown()
        {
            this.Animator.ResetTrigger("StartCooldown");
            this.Animator.ResetTrigger("StartRageCooldown");
            this.Animator.SetTrigger("FinishCooldown");
        }

        public void LockByEMP(bool isLock)
        {
            this.lockByEMPAnimator.SetBool("Locked", isLock);
        }

        public void Passive()
        {
            this.Animator.SetBool("Passive", true);
        }

        public void PressedWhenDisable()
        {
            this.Animator.SetTrigger("PressedWhenDisable");
        }

        public void RageMode()
        {
            this.Animator.SetBool("Rage", this.isRage);
        }

        public void SetCooldownCoeff(float coeff)
        {
            this.CooldownCoeff = coeff;
        }

        public void SetEpic()
        {
            this.icon.GetComponent<Image>().color = (Color) this.epicColor;
        }

        public void SetExceptional()
        {
            this.icon.GetComponent<Image>().color = (Color) this.exceptionalColor;
        }

        public void StartCooldown(float timeInSec, bool slotEnabled)
        {
            this.CDFill.color = this.CDColor;
            this.Animator.ResetTrigger("FinishCooldown");
            this.Animator.SetTrigger("StartCooldown");
            this.Animator.SetBool("Enabled", slotEnabled);
            this._cooldownMultiplier = 1f / timeInSec;
            this.Animator.SetFloat("CooldownMultiplier", this._cooldownMultiplier);
            this.cooldown.Cooldown = timeInSec;
            this.cooldownTime = timeInSec;
            this.cooldownTimer = 0f;
            this.inCooldown = true;
        }

        public void StartRageCooldown(float timeInSec, bool slotEnabled)
        {
            this.CDFill.color = this.RageCDColor;
            this.Animator.ResetTrigger("FinishCooldown");
            this.Animator.SetTrigger("StartCooldown");
            this.Animator.SetBool("Enabled", slotEnabled);
            this._cooldownMultiplier = 1f / (timeInSec / this.CooldownCoeff);
            this.Animator.SetFloat("CooldownMultiplier", this._cooldownMultiplier);
            this.cooldown.Cooldown = timeInSec;
            this.cooldownTime = timeInSec;
            this.cooldownTimer = 0f;
            this.inCooldown = true;
            this.isRage = true;
        }

        private void Update()
        {
            if (this.inCooldown)
            {
                this.cooldownTimer += this.CooldownCoeff * Time.deltaTime;
                float t = this.cooldownTimer / this.cooldownTime;
                float num2 = Mathf.Clamp01(1f - t);
                if ((this.maxItemAmmunitionCount > 0) && ((this.currentAmmunitionItemIndex < this.ammunitionBars.Length) && (Mathf.Abs((float) (this.CDFill.fillAmount - num2)) > 0f)))
                {
                    this.ammunitionBars[this.currentAmmunitionItemIndex].FillValue = t;
                }
                if (!this.isRage)
                {
                    this.CDFill.color = Color.Lerp(this.FullCDColor, this.LowCDColor, t);
                }
                this.CDFill.fillAmount = num2;
                if (this.cooldownTimer >= this.cooldownTime)
                {
                    this.inCooldown = false;
                }
            }
        }

        private UnityEngine.Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = base.GetComponent<UnityEngine.Animator>();
                }
                return this.animator;
            }
        }

        public string Icon
        {
            set => 
                this.icon.SpriteUid = value;
        }

        public string KeyBind
        {
            set => 
                this.keyBind.text = value;
        }

        public int MaxItemAmmunitionCount
        {
            get => 
                this.maxItemAmmunitionCount;
            set
            {
                this.maxItemAmmunitionCount = value;
                if (value > 1)
                {
                    this.ammunitionBars = new AmmunitionBar[value];
                    for (int i = 0; i < value; i++)
                    {
                        this.ammunitionBars[i] = Instantiate<AmmunitionBar>(this.ammunitionBarPrefab, this.barRoot.transform);
                    }
                }
            }
        }

        public int ItemAmmunitionCount
        {
            get => 
                this.currentAmmunitionItemIndex;
            set
            {
                this.ammunitionCountWasIncreased = this.currentAmmunitionItemIndex < value;
                this.currentAmmunitionItemIndex = value;
                this.Animator.SetInteger("AmmunitionCount", value);
                for (int i = 0; i < this.ammunitionBars.Length; i++)
                {
                    AmmunitionBar bar = this.ammunitionBars[i];
                    bar.FillValue = 1f;
                    if (i < value)
                    {
                        bar.Activate();
                    }
                    else
                    {
                        bar.Deactivate();
                    }
                }
            }
        }

        public bool TabPressed
        {
            set => 
                this.Animator.SetBool("IsTab", value);
        }
    }
}

