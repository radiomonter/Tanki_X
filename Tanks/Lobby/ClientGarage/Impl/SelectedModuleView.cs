namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using TMPro;
    using UnityEngine;

    public class SelectedModuleView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI moduleName;
        [SerializeField]
        private GameObject property;
        [SerializeField]
        private Transform upgrade;
        [SerializeField]
        public GameObject ResearchButton;
        [SerializeField]
        public GameObject UpgradeCRYButton;
        [SerializeField]
        private GameObject UpgradeXCRYButton;
        [SerializeField]
        private GameObject BuyBlueprints;
        [SerializeField]
        private string damageIcon;
        [SerializeField]
        private string armorIcon;
        [SerializeField]
        private LocalizedField buyCRY;
        [SerializeField]
        private LocalizedField buyXCRY;
        [SerializeField]
        private LocalizedField bonusDamage;
        [SerializeField]
        private LocalizedField bonusArmor;
        [SerializeField]
        private LocalizedField localizeLVL;
        private NewModulesScreenSystem.SelfUserMoneyNode money;
        [SerializeField]
        private TextMeshProUGUI notEnoughText;
        [SerializeField]
        private LocalizedField notEnoughActivate;
        [SerializeField]
        private LocalizedField notEnoughUpgrade;

        private int CalculateMaximumPercentSum(List<List<int>> level2PowerByTier, int slotCount)
        {
            int num = level2PowerByTier.Count - 1;
            List<int> list = level2PowerByTier[num];
            return (list[list.Count - 1] * slotCount);
        }

        private float CalculateUpgradeCoeff(List<int[]> modulesParams, int slotCount, List<List<int>> level2PowerByTier)
        {
            int num2 = this.CalculateMaximumPercentSum(level2PowerByTier, slotCount);
            return (((float) this.CollectPercentSum(modulesParams, level2PowerByTier)) / ((float) num2));
        }

        private int CollectPercentSum(List<int[]> modulesParams, List<List<int>> level2PowerByTier)
        {
            int num = 0;
            foreach (int[] numArray in modulesParams)
            {
                int num2 = numArray[0];
                int num3 = Mathf.Min(numArray[1], level2PowerByTier[num2].Count - 1);
                num += level2PowerByTier[num2][num3];
            }
            return num;
        }

        public void InitMoney(NewModulesScreenSystem.SelfUserMoneyNode money)
        {
            this.money = money;
        }

        public void ShowButton(ModuleItem item)
        {
            this.BuyBlueprints.SetActive(false);
            this.ResearchButton.SetActive(false);
            this.UpgradeCRYButton.SetActive(false);
            this.UpgradeXCRYButton.SetActive(false);
            if (item != null)
            {
                if ((item.UserItem == null) && (item.CraftPrice.Cards <= item.UserCardCount))
                {
                    this.ResearchButton.SetActive(true);
                    this.ResearchButton.GetComponent<ResearchModuleButtonComponent>().TitleTextActivate = $"{item.CraftPrice.Cards}";
                }
                else if ((item.UserItem == null) && (item.CraftPrice.Cards > item.UserCardCount))
                {
                    this.notEnoughText.text = $"{this.notEnoughActivate.Value}";
                    this.BuyBlueprints.SetActive(true);
                }
                else if ((item.UserItem == null) || ((item.UpgradePrice > item.UserCardCount) || (item.Level == item.MaxLevel)))
                {
                    if ((item.UserItem != null) && (item.UpgradePrice > item.UserCardCount))
                    {
                        this.notEnoughText.text = $"{this.notEnoughUpgrade.Value}";
                        this.BuyBlueprints.SetActive(true);
                    }
                }
                else
                {
                    this.UpgradeCRYButton.SetActive(true);
                    this.UpgradeXCRYButton.SetActive(true);
                    if (this.money.userMoney.Money >= item.UpgradePriceCRY)
                    {
                        this.UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().TitleTextUpgrade = $"{item.UpgradePriceCRY + " <sprite=8>"}";
                        this.UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().NotEnoughTextEnable = false;
                    }
                    else
                    {
                        this.UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().BuyCrystal = $"{this.buyCRY.Value}";
                        this.UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().NotEnoughText = item.UpgradePriceCRY - this.money.userMoney.Money;
                        this.UpgradeCRYButton.GetComponent<UpgradeModuleButtonComponent>().NotEnoughTextEnable = true;
                    }
                    if (this.money.userXCrystals.Money >= item.UpgradePriceXCRY)
                    {
                        this.UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().TitleTextUpgrade = $"{item.UpgradePriceXCRY + " <sprite=9>"}";
                        this.UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().NotEnoughTextEnable = false;
                    }
                    else
                    {
                        this.UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().BuyCrystal = $"{this.buyXCRY.Value}";
                        this.UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().NotEnoughText = item.UpgradePriceXCRY - this.money.userXCrystals.Money;
                        this.UpgradeXCRYButton.GetComponent<UpgradeXCryModuleButtonComponent>().NotEnoughTextEnable = true;
                    }
                }
            }
        }

        private void ShowDamageBonus(ModuleItem item, long max, long current, List<List<int>> level2PowerByTier, TankPartItem tank, TankPartItem weapon)
        {
            if (this.CalculateMaximumPercentSum(level2PowerByTier, 3) >= 0)
            {
                VisualProperty property = ((item.TankPartModuleType != null) ? weapon : tank).Properties[0];
                GameObject obj2 = Instantiate<GameObject>(this.property, this.upgrade);
                obj2.SetActive(true);
                ModulePropertyView component = obj2.GetComponent<ModulePropertyView>();
                component.PropertyName = property.Name;
                if (item.TankPartModuleType == TankPartModuleType.TANK)
                {
                    component.PropertyName = (string) this.bonusArmor;
                    component.SpriteUid = this.armorIcon;
                }
                else
                {
                    component.PropertyName = (string) this.bonusDamage;
                    component.SpriteUid = this.damageIcon;
                }
                int tierNumber = item.TierNumber;
                if (item.UserItem == null)
                {
                    List<int[]> modulesParams = new List<int[]>();
                    int[] numArray1 = new int[] { tierNumber, (int) current };
                    modulesParams.Add(numArray1);
                    List<int[]> list2 = new List<int[]>();
                    int[] numArray2 = new int[] { tierNumber, (int) max };
                    list2.Add(numArray2);
                    component.CurrentParam = property.GetValue(this.CalculateUpgradeCoeff(modulesParams, 3, level2PowerByTier)) - property.InitialValue;
                    component.MaxParam = property.GetValue(this.CalculateUpgradeCoeff(list2, 3, level2PowerByTier)) - property.InitialValue;
                }
                else if (current == max)
                {
                    List<int[]> modulesParams = new List<int[]>();
                    int[] numArray3 = new int[] { tierNumber, (int) max };
                    modulesParams.Add(numArray3);
                    float num3 = property.GetValue(this.CalculateUpgradeCoeff(modulesParams, 3, level2PowerByTier));
                    component.CurrentParam = num3 - property.InitialValue;
                    component.MaxParam = num3 - property.InitialValue;
                }
                else
                {
                    List<int[]> modulesParams = new List<int[]>();
                    int[] numArray4 = new int[] { tierNumber, (int) current };
                    modulesParams.Add(numArray4);
                    List<int[]> list5 = new List<int[]>();
                    int[] numArray5 = new int[] { tierNumber, ((int) current) + 1 };
                    list5.Add(numArray5);
                    List<int[]> list6 = new List<int[]>();
                    int[] numArray6 = new int[] { tierNumber, (int) max };
                    list6.Add(numArray6);
                    component.CurrentParam = property.GetValue(this.CalculateUpgradeCoeff(modulesParams, 3, level2PowerByTier)) - property.InitialValue;
                    component.NextParam = property.GetValue(this.CalculateUpgradeCoeff(list5, 3, level2PowerByTier)) - property.InitialValue;
                    component.MaxParam = property.GetValue(this.CalculateUpgradeCoeff(list6, 3, level2PowerByTier)) - property.InitialValue;
                }
                component.ProgressBar = true;
            }
        }

        public void UpdateView(ModuleItem moduleItem, List<List<int>> level2PowerByTier, TankPartItem tank, TankPartItem weapon)
        {
            IEnumerator enumerator = this.upgrade.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Destroy(current.gameObject);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            if (moduleItem == null)
            {
                this.moduleName.text = null;
            }
            else
            {
                long level = moduleItem.Level;
                int maxLevel = moduleItem.MaxLevel;
                this.moduleName.text = (moduleItem.UserItem != null) ? $"{moduleItem.Name} <color=#838383FF>({this.localizeLVL.Value} {(moduleItem.Level + 1L)})" : $"{moduleItem.Name}";
                this.ShowDamageBonus(moduleItem, (long) maxLevel, level, level2PowerByTier, tank, weapon);
                for (int i = 0; i < moduleItem.properties.Count; i++)
                {
                    GameObject obj2 = Instantiate<GameObject>(this.property, this.upgrade);
                    obj2.SetActive(true);
                    ModulePropertyView component = obj2.GetComponent<ModulePropertyView>();
                    ModuleVisualProperty property = moduleItem.properties[i];
                    component.SpriteUid = property.SpriteUID;
                    component.PropertyName = property.Name;
                    component.Units = property.Unit;
                    component.Format = property.Format;
                    if (property.MaxAndMin)
                    {
                        if (moduleItem.UserItem == null)
                        {
                            component.CurrentParamString = property.MaxAndMinString[(int) ((IntPtr) level)];
                        }
                        else if (level == maxLevel)
                        {
                            component.CurrentParamString = property.MaxAndMinString[(int) ((IntPtr) level)];
                        }
                        else
                        {
                            component.CurrentParamString = property.MaxAndMinString[(int) ((IntPtr) level)];
                            component.NextParamString = property.MaxAndMinString[(int) ((IntPtr) (level + 1L))];
                        }
                    }
                    else if (moduleItem.UserItem == null)
                    {
                        component.CurrentParam = property.CalculateModuleEffectPropertyValue(0, maxLevel);
                        component.MaxParam = property.CalculateModuleEffectPropertyValue(maxLevel, maxLevel);
                    }
                    else if (level == maxLevel)
                    {
                        float num6 = property.CalculateModuleEffectPropertyValue(maxLevel, maxLevel);
                        component.CurrentParam = num6;
                        component.MaxParam = num6;
                    }
                    else
                    {
                        float num7 = property.CalculateModuleEffectPropertyValue(maxLevel, maxLevel);
                        component.CurrentParam = (level == -1L) ? 0f : property.CalculateModuleEffectPropertyValue((int) level, maxLevel);
                        component.NextParam = property.CalculateModuleEffectPropertyValue(((int) level) + 1, maxLevel);
                        component.MaxParam = num7;
                    }
                    component.ProgressBar = moduleItem.properties[i].ProgressBar;
                }
            }
            this.ShowButton(moduleItem);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

