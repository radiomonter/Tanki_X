namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class QuestItemGUIComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener
    {
        private Entity entity;
        [SerializeField]
        private TextMeshProUGUI taskText;
        [SerializeField]
        private TextMeshProUGUI conditionText;
        [SerializeField]
        private Tanks.Lobby.ClientQuests.Impl.QuestProgressGUIComponent questProgressGUIComponent;
        [SerializeField]
        private Tanks.Lobby.ClientQuests.Impl.QuestRewardGUIComponent questRewardGUIComponent;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private GameObject premiumBackground;
        [SerializeField]
        private TextMeshProUGUI questsCount;
        [SerializeField]
        private GameObject changeButton;

        public void AddQuest()
        {
            this.animator.SetTrigger("AddQuest");
        }

        public void ChangeQuest()
        {
            if (this.entity != null)
            {
                this.animator.SetBool("showConfirmChangeQuest", true);
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new HideQuestsChangeMenuEvent(), this.entity);
            }
        }

        public void CompeleQuest()
        {
            this.animator.SetTrigger("CompleteQuest");
        }

        public void ConfirmChangeQuest()
        {
            if (this.entity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new ChangeQuestEvent(), this.entity);
            }
        }

        void AttachToEntityListener.AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity)
        {
            this.entity = null;
            Destroy(base.gameObject);
        }

        private void QuestRemoved()
        {
            if (this.entity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new QuestRemovedEvent(), this.entity);
            }
            else
            {
                Destroy(base.gameObject);
            }
        }

        public void RejectChangeQuest()
        {
            this.animator.SetBool("showConfirmChangeQuest", false);
        }

        public void RemoveQuest()
        {
            this.animator.SetTrigger("RemoveQuest");
        }

        public void SetChangeButtonActivity(bool active)
        {
            this.changeButton.SetActive(active);
        }

        public void SetQuestCompleted(bool value)
        {
            this.animator.SetBool("completedQuest", value);
        }

        public void SetQuestResult(bool value)
        {
            this.animator.SetBool("questResult", value);
        }

        public void ShowPremiumBack(int count)
        {
            this.premiumBackground.SetActive(true);
            this.questsCount.text = count.ToString();
        }

        public void ShowQuest()
        {
            this.animator.SetTrigger("ActivateQuest");
        }

        public Tanks.Lobby.ClientQuests.Impl.QuestProgressGUIComponent QuestProgressGUIComponent =>
            this.questProgressGUIComponent;

        public Tanks.Lobby.ClientQuests.Impl.QuestRewardGUIComponent QuestRewardGUIComponent =>
            this.questRewardGUIComponent;

        public string ConditionText
        {
            get => 
                this.conditionText.text;
            set
            {
                this.conditionText.text = value;
                this.conditionText.gameObject.SetActive(true);
            }
        }

        public string TaskText
        {
            get => 
                this.taskText.text;
            set => 
                this.taskText.text = value;
        }
    }
}

