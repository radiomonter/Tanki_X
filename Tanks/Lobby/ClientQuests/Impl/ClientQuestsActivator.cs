﻿namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientQuestsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
        }

        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register<QuestTemplate>();
            TemplateRegistry.Register<KillsInOneBattleQuestTemplate>();
            TemplateRegistry.Register<KillsInManyBattlesQuestTemplate>();
            TemplateRegistry.Register<KillsInOneBattleEveryDayQuestTemplate>();
            TemplateRegistry.Register<BattleCountQuestTemplate>();
            TemplateRegistry.Register<FlagQuestTemplate>();
            TemplateRegistry.Register<FragQuestTemplate>();
            TemplateRegistry.Register<ScoreQuestTemplate>();
            TemplateRegistry.Register<SupplyQuestTemplate>();
            TemplateRegistry.Register<WinQuestTemplate>();
            EngineService.RegisterSystem(new QuestOrderSystem());
            EngineService.RegisterSystem(new QuestsScreenSystem());
            EngineService.RegisterSystem(new QuestItemGUISystem());
            EngineService.RegisterSystem(new QuestsRewardBufferSystem());
            EngineService.RegisterSystem(new QuestsButtonNotificationBadgeSystem());
            EngineService.RegisterSystem(new ChangeQuestSystem());
            TemplateRegistry.Register<BattleQuestTemplate>();
            EngineService.RegisterSystem(new InBattleQuestsScreenSystem());
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

