namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class ExitDialogSystem : ECSSystem
    {
        [OnEventFire]
        public void ExitDialogRewards(NodeAddedEvent e, DailyBonusReturnDialogNode dialog, [JoinAll] UserDailyBonusNode user, [JoinAll] DailyBonusConfig dailyBonusConfig)
        {
            bool flag = false;
            if (user.userStatistics.Statistics.ContainsKey("BATTLES_PARTICIPATED"))
            {
                flag = user.userStatistics.Statistics["BATTLES_PARTICIPATED"] >= dailyBonusConfig.dailyBonusCommonConfig.BattleCountToUnlockDailyBonuses;
            }
            if (!flag)
            {
                dialog.exitGameDialog.content.SetActive(false);
            }
            else
            {
                dialog.exitGameDialog.content.SetActive(true);
                DailyBonusFirstCycleComponent dailyBonusFirstCycle = dailyBonusConfig.dailyBonusFirstCycle;
                DailyBonusEndlessCycleComponent dailyBonusEndlessCycle = dailyBonusConfig.dailyBonusEndlessCycle;
                int num = 0;
                DailyBonusData bonus = dailyBonusConfig.dailyBonusFirstCycle.DailyBonuses[0];
                bool flag2 = true;
                dialog.exitGameDialog.ReceivedRewards.AddRange(user.userDailyBonusReceivedRewards.ReceivedRewards);
                if (user.userDailyBonusCycle.CycleNumber == 0L)
                {
                    num = dailyBonusFirstCycle.Zones[(int) ((IntPtr) user.userDailyBonusZone.ZoneNumber)];
                    if (user.userDailyBonusReceivedRewards.ReceivedRewards.Count == (num + 1))
                    {
                        if ((user.userDailyBonusZone.ZoneNumber + 1L) < dailyBonusFirstCycle.Zones.Length)
                        {
                            num = dailyBonusFirstCycle.Zones[(int) ((IntPtr) (user.userDailyBonusZone.ZoneNumber + 1L))];
                        }
                        else
                        {
                            flag2 = false;
                            num = dailyBonusEndlessCycle.Zones[0];
                            dialog.exitGameDialog.ReceivedRewards.Clear();
                        }
                    }
                }
                else
                {
                    num = dailyBonusEndlessCycle.Zones[(int) ((IntPtr) user.userDailyBonusZone.ZoneNumber)];
                    flag2 = false;
                    if (user.userDailyBonusReceivedRewards.ReceivedRewards.Count == (num + 1))
                    {
                        if ((user.userDailyBonusZone.ZoneNumber + 1L) < dailyBonusEndlessCycle.Zones.Length)
                        {
                            num = dailyBonusEndlessCycle.Zones[(int) ((IntPtr) (user.userDailyBonusZone.ZoneNumber + 1L))];
                        }
                        else
                        {
                            num = dailyBonusEndlessCycle.Zones[0];
                            dialog.exitGameDialog.ReceivedRewards.Clear();
                        }
                    }
                }
                for (int i = 0; i <= num; i++)
                {
                    DailyBonusData data2 = !flag2 ? dailyBonusConfig.dailyBonusEndlessCycle.DailyBonuses[i] : dailyBonusConfig.dailyBonusFirstCycle.DailyBonuses[i];
                    if (!dialog.exitGameDialog.ReceivedRewards.Contains(data2.Code))
                    {
                        if (data2.DailyBonusType == DailyBonusType.CONTAINER)
                        {
                            bonus = data2;
                        }
                        if ((bonus.DailyBonusType != DailyBonusType.CONTAINER) && (data2.DailyBonusType == DailyBonusType.DETAIL))
                        {
                            bonus = data2;
                        }
                        if ((bonus.DailyBonusType != DailyBonusType.CONTAINER) && ((bonus.DailyBonusType != DailyBonusType.DETAIL) && (data2.DailyBonusType == DailyBonusType.XCRY)))
                        {
                            bonus = data2;
                        }
                        if ((bonus.DailyBonusType != DailyBonusType.CONTAINER) && ((bonus.DailyBonusType != DailyBonusType.DETAIL) && ((bonus.DailyBonusType != DailyBonusType.XCRY) && (data2.DailyBonusType == DailyBonusType.ENERGY))))
                        {
                            bonus = data2;
                        }
                        if ((bonus.DailyBonusType != DailyBonusType.CONTAINER) && ((bonus.DailyBonusType != DailyBonusType.DETAIL) && ((bonus.DailyBonusType != DailyBonusType.XCRY) && ((bonus.DailyBonusType != DailyBonusType.ENERGY) && ((data2.DailyBonusType == DailyBonusType.CRY) && (data2.CryAmount >= bonus.CryAmount))))))
                        {
                            bonus = data2;
                        }
                    }
                }
                this.InstantiateBonus(dialog, bonus);
            }
        }

        [OnEventFire]
        public void ExitDialogTimer(UpdateEvent e, DailyBonusReturnDialogNode dialog, [JoinAll] UserDailyBonusNode user, [JoinAll] DailyBonusConfig dailyBonusConfig)
        {
            float num = (float) (user.userDailyBonusNextReceivingDate.Date - Date.Now);
            string str = Mathf.Floor((num / 60f) % 60f).ToString("00");
            string str2 = (num % 60f).ToString("00");
            string str3 = Mathf.Floor((num / 60f) / 60f).ToString("00");
            if (num <= 0f)
            {
                int index = 0;
                while (true)
                {
                    if (index >= dialog.exitGameDialog.textNotReady.Length)
                    {
                        dialog.exitGameDialog.textReady.SetActive(true);
                        break;
                    }
                    dialog.exitGameDialog.textNotReady[index].SetActive(false);
                    index++;
                }
            }
            else
            {
                int index = 0;
                while (true)
                {
                    if (index >= dialog.exitGameDialog.textNotReady.Length)
                    {
                        dialog.exitGameDialog.textReady.SetActive(false);
                        dialog.exitGameDialog.timer.text = $"{str3:0}:{str:00}:{str2:00}";
                        break;
                    }
                    dialog.exitGameDialog.textNotReady[index].SetActive(true);
                    index++;
                }
            }
        }

        private void InstantiateBonus(DailyBonusReturnDialogNode dialog, DailyBonusData bonus)
        {
            DailyBonusType dailyBonusType = bonus.DailyBonusType;
            switch (dailyBonusType)
            {
                case DailyBonusType.CRY:
                    dialog.exitGameDialog.InstantiateCryBonus(bonus.CryAmount);
                    break;

                case DailyBonusType.XCRY:
                    dialog.exitGameDialog.InstantiateXCryBonus(bonus.XcryAmount);
                    break;

                case DailyBonusType.ENERGY:
                    dialog.exitGameDialog.InstantiateEnergyBonus(bonus.EnergyAmount);
                    break;

                case DailyBonusType.CONTAINER:
                    dialog.exitGameDialog.InstantiateContainerBonus(bonus.ContainerReward.MarketItemId);
                    break;

                default:
                    if (dailyBonusType == DailyBonusType.DETAIL)
                    {
                        dialog.exitGameDialog.InstantiateDetailBonus(bonus.DetailReward.MarketItemId);
                    }
                    break;
            }
        }

        public class DailyBonusConfig : Node
        {
            public DailyBonusCommonConfigComponent dailyBonusCommonConfig;
            public DailyBonusFirstCycleComponent dailyBonusFirstCycle;
            public DailyBonusEndlessCycleComponent dailyBonusEndlessCycle;
        }

        public class DailyBonusReturnDialogNode : Node
        {
            public ExitGameDialogComponent exitGameDialog;
        }

        public class UserDailyBonusNode : Node
        {
            public UserDailyBonusInitializedComponent userDailyBonusInitialized;
            public UserDailyBonusCycleComponent userDailyBonusCycle;
            public UserDailyBonusReceivedRewardsComponent userDailyBonusReceivedRewards;
            public UserDailyBonusZoneComponent userDailyBonusZone;
            public UserDailyBonusNextReceivingDateComponent userDailyBonusNextReceivingDate;
            public UserStatisticsComponent userStatistics;
        }
    }
}

