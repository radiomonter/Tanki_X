namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class ElevatedAccessUserBaseSystem : ECSSystem
    {
        protected Entity user;

        protected void AddBotsToBattle(string parameters)
        {
            int num;
            TeamColor color;
            try
            {
                char[] separator = new char[] { ' ' };
                num = int.Parse(parameters.Split(separator)[2]);
                char[] chArray2 = new char[] { ' ' };
                string str = parameters.Split(chArray2)[1];
                color = (TeamColor) Enum.Parse(typeof(TeamColor), str);
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }
            ElevatedAccessUserAddBotsToBattleEvent eventInstance = new ElevatedAccessUserAddBotsToBattleEvent {
                TeamColor = color,
                Count = num
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void AddKills(string parameters)
        {
            int num;
            try
            {
                char[] separator = new char[] { ' ' };
                num = int.Parse(parameters.Split(separator)[1]);
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }
            ElevatedAccessUserAddKillsEvent eventInstance = new ElevatedAccessUserAddKillsEvent {
                Count = num
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void AddScore(string parameters)
        {
            int num = 0;
            try
            {
                char[] separator = new char[] { ' ' };
                num = int.Parse(parameters.Split(separator)[1]);
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }
            ElevatedAccessUserAddScoreEvent eventInstance = new ElevatedAccessUserAddScoreEvent {
                Count = num
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void BanUser(string parameters)
        {
            ElevatedAccessUserBanUserEvent eventInstance = this.Punish<ElevatedAccessUserBanUserEvent>(parameters);
            if (eventInstance != null)
            {
                string str = string.Empty;
                try
                {
                    char[] separator = new char[] { ' ' };
                    str = parameters.Split(separator)[3];
                }
                catch (Exception)
                {
                    SmartConsole.WriteLine("Wrong parameters");
                }
                eventInstance.Type = str;
                base.ScheduleEvent(eventInstance, this.user);
            }
        }

        protected void BlockUser(string parameters)
        {
            base.ScheduleEvent(this.Punish<ElevatedAccessUserBlockUserEvent>(parameters), this.user);
        }

        protected void ChangeEnergy(string parameters)
        {
            int num = 0;
            try
            {
                char[] separator = new char[] { ' ' };
                num = int.Parse(parameters.Split(separator)[1]);
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }
            ElevatedAccessUserChangeEnergyEvent eventInstance = new ElevatedAccessUserChangeEnergyEvent {
                Count = num
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void ChangeReputation(string parameters)
        {
            int num = 0;
            try
            {
                char[] separator = new char[] { ' ' };
                num = int.Parse(parameters.Split(separator)[1]);
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }
            ElevatedAccessUserChangeReputationEvent eventInstance = new ElevatedAccessUserChangeReputationEvent {
                Count = num
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void CreateUserItem(string parameters)
        {
            long num = 0L;
            long num2 = 0L;
            try
            {
                char[] separator = new char[] { ' ' };
                num = long.Parse(parameters.Split(separator)[2]);
                char[] chArray2 = new char[] { ' ' };
                num2 = long.Parse(parameters.Split(chArray2)[1]);
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }
            ElevatedAccessUserCreateItemEvent eventInstance = new ElevatedAccessUserCreateItemEvent {
                Count = num,
                ItemId = num2
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void DropGold(string parameters)
        {
            GoldType tELEPORT = GoldType.TELEPORT;
            try
            {
                tELEPORT = this.ExtractTypeFromParams<GoldType>(parameters);
            }
            catch (Exception)
            {
            }
            ElevatedAccessUserDropSupplyGoldEvent eventInstance = new ElevatedAccessUserDropSupplyGoldEvent {
                GoldType = tELEPORT
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void DropSupply(string parameters)
        {
            ElevatedAccessUserDropSupplyEvent eventInstance = new ElevatedAccessUserDropSupplyEvent {
                BonusType = this.ExtractTypeFromParams<BonusType>(parameters)
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        private T ExtractTypeFromParams<T>(string parameters) where T: struct, IConvertible
        {
            char[] separator = new char[] { ' ' };
            string str = parameters.Split(separator)[1];
            return (T) Enum.Parse(typeof(T), str);
        }

        protected void FinishBattle(string parameters)
        {
            base.ScheduleEvent(new ElevatedAccessUserFinishBattleEvent(), this.user);
        }

        protected void GiveBattleQuest(string parameters)
        {
            string str = string.Empty;
            try
            {
                char[] separator = new char[] { ' ' };
                str = parameters.Split(separator)[1];
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameters");
            }
            ElevatedAccessUserGiveBattleQuestEvent eventInstance = new ElevatedAccessUserGiveBattleQuestEvent {
                QuestPath = str
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void IncreaseScore(string parameters)
        {
            char[] separator = new char[] { ' ' };
            string str = parameters.Split(separator)[1];
            int num = 1;
            try
            {
                char[] chArray2 = new char[] { ' ' };
                num = int.Parse(parameters.Split(chArray2)[2]);
            }
            catch (Exception)
            {
            }
            ElevatedAccessUserIncreaseScoreEvent eventInstance = new ElevatedAccessUserIncreaseScoreEvent {
                TeamColor = (TeamColor) Enum.Parse(typeof(TeamColor), str),
                Amount = num
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void InitConsole(SelfUserNode selfUser)
        {
            this.user = selfUser.Entity;
            if (Object.FindObjectOfType<SmartConsole>() == null)
            {
                Object.Instantiate<GameObject>(Object.FindObjectOfType<SmartConsoleActivator>().SmartConsole);
                SmartConsole.RegisterCommand("changeEnergy", "changeEnergy -10", "change user energy", new SmartConsole.ConsoleCommandFunction(this.ChangeEnergy));
                SmartConsole.RegisterCommand("incScore", "increaseScore RED", "Increases score of a team", new SmartConsole.ConsoleCommandFunction(this.IncreaseScore));
                SmartConsole.RegisterCommand("finish", "finish", "Finish current battle", new SmartConsole.ConsoleCommandFunction(this.FinishBattle));
                SmartConsole.RegisterCommand("banUser", "banUser User1 CHEATING DAY", "Bans user chat for REASON for TYPE. Possible reasons: FLOOD, FOUL, SEX, POLITICS... Possible type: NONE, WARN, MINUTE, HOUR, DAY...", new SmartConsole.ConsoleCommandFunction(this.BanUser));
                SmartConsole.RegisterCommand("unbanUser", "unbanUser User1", "Unbans user chat", new SmartConsole.ConsoleCommandFunction(this.UnbanUser));
                SmartConsole.RegisterCommand("addScore", "addScore 100", "Add user score", new SmartConsole.ConsoleCommandFunction(this.AddScore));
                SmartConsole.RegisterCommand("addKills", "addKills 10", "Add kills to user", new SmartConsole.ConsoleCommandFunction(this.AddKills));
                SmartConsole.RegisterCommand("giveBattleQuest", "giveBattleQuest tutorial/supply", "give battle quest to user", new SmartConsole.ConsoleCommandFunction(this.GiveBattleQuest));
                SmartConsole.RegisterCommand("changeReputation", "changeReputation -10", "change reputation", new SmartConsole.ConsoleCommandFunction(this.ChangeReputation));
            }
        }

        private T Punish<T>(string parameters) where T: ElevatedAccessUserBasePunishEvent, new()
        {
            string str = string.Empty;
            string str2 = string.Empty;
            try
            {
                char[] separator = new char[] { ' ' };
                str = parameters.Split(separator)[1];
                char[] chArray2 = new char[] { ' ' };
                str2 = parameters.Split(chArray2)[2];
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameters");
                return null;
            }
            T local2 = Activator.CreateInstance<T>();
            local2.Uid = str;
            local2.Reason = str2;
            return local2;
        }

        protected void RunCommand(string parameters)
        {
            ElevatedAccessUserRunCommandEvent eventInstance = new ElevatedAccessUserRunCommandEvent {
                Command = parameters.Replace("runCommand ", string.Empty)
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void ThrowNullPointer(string parameters)
        {
            throw new NullReferenceException();
        }

        protected void UnbanUser(string parameters)
        {
            string str = string.Empty;
            try
            {
                char[] separator = new char[] { ' ' };
                str = parameters.Split(separator)[1];
            }
            catch (Exception)
            {
                SmartConsole.WriteLine("Wrong parameters");
            }
            ElevatedAccessUserUnbanUserEvent eventInstance = new ElevatedAccessUserUnbanUserEvent {
                Uid = str
            };
            base.ScheduleEvent(eventInstance, this.user);
        }

        protected void WipeUserItems(string parameters)
        {
            base.ScheduleEvent(new ElevatedAccessUserWipeUserItemsEvent(), this.user);
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

