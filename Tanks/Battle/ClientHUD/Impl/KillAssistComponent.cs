namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientControls.Impl;
    using UnityEngine;

    public class KillAssistComponent : BehaviourComponent
    {
        public CombatEventLog combatEventLog;
        public AnimatedLong scoreTotalNumber;
        public Animator totalNumberAnimator;
        public LocalizedField flagDeliveryMessage;
        public LocalizedField flagReturnMessage;
        public LocalizedField killMessage;
        public LocalizedField assistMessage;
        public LocalizedField healMessage;
        public LocalizedField streakMessage;
        private bool visible;

        public void AddAssistMessage(int score, int percent, string nickname)
        {
            this.IncreaseTotalScore(score);
            string messageText = this.assistMessage.Value.Replace("{scoreValule}", score.ToString()).Replace("{percent}", percent.ToString()).Replace("{killer}", this.ParseNickname(nickname));
            this.combatEventLog.AddMessage(messageText);
        }

        public void AddFlagDeliveryMessage(int score)
        {
            this.IncreaseTotalScore(score);
            string messageText = this.flagDeliveryMessage.Value.Replace("{scoreValule}", score.ToString());
            this.combatEventLog.AddMessage(messageText);
        }

        public void AddFlagReturnMessage(int score)
        {
            this.IncreaseTotalScore(score);
            string messageText = this.flagReturnMessage.Value.Replace("{scoreValule}", score.ToString());
            this.combatEventLog.AddMessage(messageText);
        }

        public void AddHealMessage(int score)
        {
            this.IncreaseTotalScore(score);
            string messageText = this.healMessage.Value.Replace("{scoreValule}", score.ToString());
            this.combatEventLog.AddMessage(messageText);
        }

        public void AddKillMessage(int score, string nickname, int rank)
        {
            this.IncreaseTotalScore(score);
            string messageText = CombatEventLogUtil.ApplyPlaceholder(this.killMessage.Value.Replace("{scoreValule}", score.ToString()), "{killer}", rank, this.ParseNickname(nickname), Color.white);
            this.combatEventLog.AddMessage(messageText);
        }

        public void AddKillStreakMessage(int score)
        {
            this.IncreaseTotalScore(score);
            string messageText = this.streakMessage.Value.Replace("{scoreValule}", score.ToString());
            this.combatEventLog.AddMessage(messageText);
        }

        private void Awake()
        {
            this.scoreTotalNumber.Value = 0L;
        }

        public void Clear()
        {
            this.SetTotalNumberToZero();
            this.combatEventLog.Clear();
        }

        private void IncreaseTotalScore(int score)
        {
            this.scoreTotalNumber.Value += score;
            this.totalNumberAnimator.SetBool("Visible", this.visible);
            this.totalNumberAnimator.SetTrigger("Show");
        }

        private string ParseNickname(string nickname) => 
            nickname.Replace("botxz_", string.Empty);

        public void SetTotalNumberToZero()
        {
            this.scoreTotalNumber.Value = 0L;
        }

        public void SetVisible(bool visible)
        {
            this.visible = visible;
        }

        public void ShowRandomAssistMessage()
        {
            string[] strArray = new string[] { "Deathcraft", "OMOEWAMOE_SHIRANEU", "devochka", "kit", "Nagib-na-smoke" };
            int rank = Random.Range(1, 0x65);
            string nickname = strArray[Random.Range(0, strArray.Length)];
            int num2 = Random.Range(0, 4);
            if (num2 == 0)
            {
                this.AddKillMessage(Random.Range(20, 40), nickname, rank);
            }
            else if (num2 == 1)
            {
                this.AddAssistMessage(Random.Range(1, 20), Random.Range(10, 50), nickname);
            }
            else if (num2 == 2)
            {
                this.AddFlagDeliveryMessage(Random.Range(20, 40));
            }
            else
            {
                this.AddFlagReturnMessage(Random.Range(20, 40));
            }
        }
    }
}

