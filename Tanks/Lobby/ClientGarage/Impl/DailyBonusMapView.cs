namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class DailyBonusMapView : MonoBehaviour
    {
        public List<GameObject> zones;
        public GameObject bonusElementPrefab;
        public List<MapViewBonusElement> bonusElements;
        public MapViewBonusElement selectedBonusElement;
        public Action<MapViewBonusElement> onSelectedBonusElementChanged;
        public ImageListSkin back;
        private static string DAILY_BONUS_CYCLE_KEY = "DAILY_BONUS_CYCLE_KEY";
        private static string DAILY_BONUS_ELEMENT_LOCATION_KEY = "DAILY_BONUS_ELEMENT_LOCATION_KEY";

        private void CreateBonusElements(DailyBonusCycleComponent dailyBonusCycleComponent, DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode)
        {
            int num = 0;
            int[] zones = dailyBonusCycleComponent.Zones;
            int cycleNumber = (int) userDailyBonusNode.userDailyBonusCycle.CycleNumber;
            int index = 0;
            while (index < zones.Length)
            {
                Random.InitState(cycleNumber);
                int num4 = num;
                while (true)
                {
                    if (num4 > zones[index])
                    {
                        num = zones[index] + 1;
                        index++;
                        break;
                    }
                    GameObject obj2 = Instantiate<GameObject>(this.bonusElementPrefab);
                    obj2.transform.SetParent(base.transform, false);
                    MapViewBonusElement component = obj2.GetComponent<MapViewBonusElement>();
                    component.ZoneIndex = index;
                    this.bonusElements.Add(component);
                    component.OnValueChanged(new Action<MapViewBonusElement, bool>(this.OnBonusElementToggleValueChanged));
                    component.SetToggleGroup(base.GetComponent<ToggleGroup>());
                    num4++;
                }
            }
        }

        private Vector2 CreateRandomLocation(int elementZoneIndex, List<List<Vector2>> zoneToPossiblePositions)
        {
            List<Vector2> list = zoneToPossiblePositions[elementZoneIndex];
            if (list.Count == 0)
            {
                return Vector2.zero;
            }
            int index = ((int) Random.value) * (list.Count - 1);
            Vector2 vector = list[index];
            list.RemoveAt(index);
            return vector;
        }

        private bool CurrentCycleSavedInPlayerPrefs(int cycleNumber) => 
            PlayerPrefs.HasKey(DAILY_BONUS_CYCLE_KEY) && PlayerPrefs.GetInt(DAILY_BONUS_CYCLE_KEY).Equals(cycleNumber);

        private Vector2 GetLocation(int elementIndex, int elementZoneIndex, int cycleNumber, List<List<Vector2>> zoneToPossiblePositions)
        {
            Vector2 savedLocation;
            if (this.HasSavedLocation(cycleNumber, elementIndex))
            {
                savedLocation = this.GetSavedLocation(elementIndex);
            }
            else
            {
                savedLocation = this.CreateRandomLocation(elementZoneIndex, zoneToPossiblePositions);
                this.SaveLocation(elementIndex, savedLocation);
            }
            return (savedLocation * this.GetZoneRadius(elementZoneIndex));
        }

        private static string GetPrefsKey(int elementIndex) => 
            DAILY_BONUS_ELEMENT_LOCATION_KEY + elementIndex;

        private Vector2 GetSavedLocation(int elementIndex)
        {
            string str = PlayerPrefs.GetString(DAILY_BONUS_ELEMENT_LOCATION_KEY + elementIndex);
            try
            {
                char[] separator = new char[] { '|' };
                string[] strArray = str.Split(separator);
                return new Vector2(float.Parse(strArray[0]), float.Parse(strArray[1]));
            }
            catch (Exception exception1)
            {
                Debug.LogException(exception1);
                PlayerPrefs.DeleteKey(DAILY_BONUS_ELEMENT_LOCATION_KEY + elementIndex);
                return new Vector2();
            }
        }

        private int GetZoneIndex(Vector2 pos, int elementRadius, int[] zoneRadiuses)
        {
            for (int i = 0; i < zoneRadiuses.Length; i++)
            {
                if (pos.magnitude <= (zoneRadiuses[i] - elementRadius))
                {
                    return i;
                }
                if (pos.magnitude <= (zoneRadiuses[i] + elementRadius))
                {
                    return -1;
                }
            }
            return -1;
        }

        private int GetZoneRadius(int zoneIndex) => 
            (int) (this.zones[zoneIndex].GetComponent<RectTransform>().rect.width / 2f);

        private int[] GetZoneRadiuses()
        {
            int[] numArray = new int[this.zones.Count];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = this.GetZoneRadius(i);
            }
            return numArray;
        }

        private List<List<Vector2>> GetZoneToPossibleNormalizedPositions()
        {
            List<List<Vector2>> list = new List<List<Vector2>>();
            for (int i = 0; i < this.zones.Count; i++)
            {
                list.Add(new List<Vector2>());
            }
            Rect rect = this.bonusElementPrefab.GetComponent<RectTransform>().rect;
            int[] zoneRadiuses = this.GetZoneRadiuses();
            int num2 = zoneRadiuses[zoneRadiuses.Length - 1];
            int width = (int) rect.width;
            int num4 = -num2;
            while (num4 < num2)
            {
                int num5 = -num2;
                while (true)
                {
                    if (num5 >= num2)
                    {
                        num4 += width;
                        break;
                    }
                    Vector2 pos = new Vector2((float) num4, (float) num5);
                    int index = this.GetZoneIndex(pos, width, zoneRadiuses);
                    if (index >= 0)
                    {
                        pos /= (float) zoneRadiuses[index];
                        list[index].Add(pos);
                    }
                    num5 += width;
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                Shuffle<Vector2>(list[j]);
            }
            return list;
        }

        private bool HasSavedLocation(int cycleNumber, int elementIndex) => 
            this.CurrentCycleSavedInPlayerPrefs(cycleNumber) && PlayerPrefs.HasKey(GetPrefsKey(elementIndex));

        public void OnBonusElementToggleValueChanged(MapViewBonusElement bonusElement, bool isChecked)
        {
            ToggleGroup component = base.GetComponent<ToggleGroup>();
            if (!isChecked)
            {
                if (!component.AnyTogglesOn())
                {
                    this.selectedBonusElement = null;
                    this.onSelectedBonusElementChanged(this.selectedBonusElement);
                }
            }
            else
            {
                this.selectedBonusElement = bonusElement;
                this.onSelectedBonusElementChanged(this.selectedBonusElement);
                foreach (Toggle toggle in component.ActiveToggles())
                {
                    if (toggle != this.selectedBonusElement.Toggle)
                    {
                        toggle.isOn = false;
                    }
                }
                if (isChecked)
                {
                    UISoundEffectController.UITransformRoot.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayClick();
                }
            }
        }

        private void SaveLocation(int elementIndex, Vector2 location)
        {
            PlayerPrefs.SetString(GetPrefsKey(elementIndex), location.x + "|" + location.y);
        }

        public static void Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int num2 = (int) (Random.value * (count + 1));
                T local = list[num2];
                list[num2] = list[count];
                list[count] = local;
            }
        }

        private void UpdateBackView(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode)
        {
            long cycleNumber = userDailyBonusNode.userDailyBonusCycle.CycleNumber;
            long num2 = (userDailyBonusNode.Entity.Id + cycleNumber) % ((long) this.back.Count);
            this.back.SelectedSpriteIndex = (int) num2;
        }

        private void UpdateBonusElementsPositions(int cycleNumber)
        {
            List<List<Vector2>> zoneToPossibleNormalizedPositions = this.GetZoneToPossibleNormalizedPositions();
            for (int i = 0; i < this.bonusElements.Count; i++)
            {
                this.bonusElements[i].GetComponent<RectTransform>().anchoredPosition = this.GetLocation(i, this.bonusElements[i].ZoneIndex, cycleNumber, zoneToPossibleNormalizedPositions);
            }
        }

        private void UpdateBonusElementView(DailyBonusCycleComponent dailyBonusCycleComponent, DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, int zoneIndex, DailyBonusData[] dailyBonuses)
        {
            List<long> receivedRewards = userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards;
            int num = dailyBonusCycleComponent.Zones[zoneIndex];
            for (int i = 0; i < dailyBonuses.Length; i++)
            {
                if (i > num)
                {
                    this.bonusElements[i].UpdateView(dailyBonuses[i], BonusElementState.INACCESSIBLE);
                }
                else if (receivedRewards.Contains(dailyBonuses[i].Code))
                {
                    this.bonusElements[i].UpdateView(dailyBonuses[i], BonusElementState.TAKEN);
                }
                else
                {
                    this.bonusElements[i].UpdateView(dailyBonuses[i], BonusElementState.ACCESSIBLE);
                    this.bonusElements[i].transform.SetAsLastSibling();
                }
            }
            if (this.selectedBonusElement != null)
            {
                this.selectedBonusElement = null;
                this.onSelectedBonusElementChanged(this.selectedBonusElement);
            }
        }

        public void UpdateInteractable(DailyBonusTeleportState state)
        {
            foreach (MapViewBonusElement element in this.bonusElements)
            {
                element.Interactable = state == DailyBonusTeleportState.Active;
            }
        }

        public void UpdateView(DailyBonusCycleComponent dailyBonusCycleComponent, DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode)
        {
            DailyBonusData[] dailyBonuses = dailyBonusCycleComponent.DailyBonuses;
            int cycleNumber = (int) userDailyBonusNode.userDailyBonusCycle.CycleNumber;
            if (this.bonusElements.Count == 0)
            {
                this.CreateBonusElements(dailyBonusCycleComponent, userDailyBonusNode);
            }
            this.UpdateBonusElementsPositions(cycleNumber);
            PlayerPrefs.SetInt(DAILY_BONUS_CYCLE_KEY, cycleNumber);
            PlayerPrefs.Save();
            int zoneNumber = (int) userDailyBonusNode.userDailyBonusZone.ZoneNumber;
            this.UpdateZoneRadiusView(zoneNumber);
            this.UpdateBonusElementView(dailyBonusCycleComponent, userDailyBonusNode, zoneNumber, dailyBonuses);
            this.UpdateBackView(userDailyBonusNode);
        }

        private void UpdateZoneRadiusView(int zoneIndex)
        {
            zoneIndex = Math.Min(zoneIndex, this.zones.Count - 1);
            int num = -1;
            int num2 = 0;
            while (true)
            {
                if (num2 < this.zones.Count)
                {
                    if (!this.zones[num2].activeSelf)
                    {
                        num2++;
                        continue;
                    }
                    num = num2;
                }
                if (num != zoneIndex)
                {
                    if (num >= 0)
                    {
                        <UpdateZoneRadiusView>c__AnonStorey0 storey = new <UpdateZoneRadiusView>c__AnonStorey0 {
                            activeZone = this.zones[num]
                        };
                        storey.activeZone.GetComponent<AnimationEventListener>().SetHideHandler(new Action(storey.<>m__0));
                        storey.activeZone.GetComponent<Animator>().SetTrigger("hide");
                    }
                    this.zones[zoneIndex].SetActive(true);
                }
                return;
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateZoneRadiusView>c__AnonStorey0
        {
            internal GameObject activeZone;

            internal void <>m__0()
            {
                this.activeZone.SetActive(false);
            }
        }
    }
}

