namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class MainHUDComponent : BehaviourComponent
    {
        [SerializeField]
        private HPBar hpBar;
        [SerializeField]
        private HPBarGlow hpBar2;
        [SerializeField]
        private EnergyBar energyBar;
        [SerializeField]
        private EnergyBarGlow energyBar2;
        public BattleHudRootComponent battleHudRoot;
        [SerializeField]
        private TextAnimation message;
        private bool isShow;
        private SortedList<int, string> messages = new SortedList<int, string>();
        private bool activated;
        [SerializeField]
        private GameObject battleLog;
        [SerializeField]
        private GameObject inventory;

        public void Activate()
        {
            Canvas componentInParent = base.GetComponentInParent<Canvas>();
            componentInParent.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            componentInParent.planeDistance = 10f;
            base.GetComponent<Animator>().SetTrigger("Show");
        }

        private void AfterActivation()
        {
            this.isShow = true;
            this.activated = true;
            if (this.messages.Count > 0)
            {
                this.message.Text = this.messages.Values[this.messages.Count - 1];
            }
            base.Invoke("EnableBattleLog", 1f);
        }

        private void EnableBattleLog()
        {
            this.battleLog.SetActive(true);
        }

        public void EnergyBlink(bool value)
        {
            this.energyBar.Blink(value);
            this.energyBar2.Blink(value);
        }

        public void EnergyInjectionBlink(bool value)
        {
            this.energyBar.EnergyInjectionBlink(value);
            this.energyBar2.EnergyInjectionBlink(value);
        }

        public void Hide()
        {
            base.GetComponent<Animator>().SetTrigger("Hide");
            this.isShow = false;
        }

        private void OnDisable()
        {
            base.CancelInvoke();
            this.messages.Clear();
            this.activated = false;
            this.battleLog.SetActive(false);
        }

        public void RemoveMessageByPriority(int priority)
        {
            this.messages.Remove(priority);
        }

        public void SetMessageCTFPosition()
        {
            this.message.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -51.5f);
        }

        public void SetMessageTDMPosition()
        {
            this.message.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -12f);
        }

        public void SetSpecatatorMode()
        {
            this.hpBar.gameObject.SetActive(false);
            this.hpBar2.gameObject.SetActive(false);
            this.EnergyBarEnabled = false;
            this.inventory.SetActive(false);
            MainHUDVersionSwitcher component = base.GetComponent<MainHUDVersionSwitcher>();
            component.specMode = true;
            component.SetCurrentHud();
        }

        public void SetTankMode()
        {
            this.hpBar.gameObject.SetActive(true);
            this.hpBar2.gameObject.SetActive(true);
            this.EnergyBarEnabled = true;
            this.inventory.SetActive(true);
            MainHUDVersionSwitcher component = base.GetComponent<MainHUDVersionSwitcher>();
            component.specMode = false;
            component.SetCurrentHud();
        }

        public void ShowMessageWithPriority(string message, int priority = 0)
        {
            if (this.messages.ContainsKey(priority))
            {
                this.messages[priority] = message;
            }
            else
            {
                this.messages.Add(priority, message);
            }
            if (this.activated)
            {
                this.message.Text = this.messages.Values[this.messages.Count - 1];
            }
        }

        public void StopEnergyBlink()
        {
            this.energyBar.StopBlinking();
            this.energyBar2.StopBlinking();
        }

        private void Update()
        {
            bool flag = Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt);
            if (Input.GetKeyDown(KeyCode.Slash) && flag)
            {
                if (this.isShow)
                {
                    this.Hide();
                }
                else
                {
                    this.Activate();
                }
            }
        }

        public float CurrentHpValue
        {
            set
            {
                this.hpBar.CurrentValue = value;
                this.hpBar2.CurrentValue = value;
            }
        }

        public float MaxHpValue
        {
            set
            {
                this.hpBar.MaxValue = value;
                this.hpBar2.MaxValue = value;
            }
        }

        public float CurrentEnergyValue
        {
            get => 
                this.energyBar.CurrentValue;
            set
            {
                this.energyBar.CurrentValue = value;
                this.energyBar2.CurrentValue = value;
            }
        }

        public float MaxEnergyValue
        {
            get => 
                this.energyBar.MaxValue;
            set
            {
                this.energyBar.MaxValue = value;
                this.energyBar2.MaxValue = value;
            }
        }

        public float EnergyAmountPerSegment
        {
            set
            {
                this.energyBar.AmountPerSegment = value;
                this.energyBar2.AmountPerSegment = value;
            }
        }

        public bool EnergyBarEnabled
        {
            set
            {
                this.energyBar.gameObject.SetActive(value);
                this.energyBar2.gameObject.SetActive(value);
            }
        }

        public long HullId
        {
            set => 
                this.hpBar.HullId = value;
        }

        public long TurretId
        {
            set => 
                this.energyBar.TurretId = value;
        }
    }
}

