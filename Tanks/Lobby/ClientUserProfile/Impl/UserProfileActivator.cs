namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientUserProfile.API;

    public class UserProfileActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            Engine engine = ECSBehaviour.EngineService.Engine;
            engine.CreateEntity(typeof(EmailConfigTemplate), "lobby/userprofile/email/configuration");
            engine.CreateEntity(typeof(DailyBonusConfigTemplate), "dailybonus");
        }

        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register<DailyBonusConfigTemplate>();
            TemplateRegistry.RegisterPart<ClientSessionTemplatePart>();
            TemplateRegistry.Register<ProfileScreenTemplate>();
            TemplateRegistry.Register<UserTemplate>();
            TemplateRegistry.RegisterPart<UserNotificationsTemplatePart>();
            TemplateRegistry.Register<NewsItemTemplate>();
            TemplateRegistry.Register<ConfiguredNewsItemTemplate>();
            TemplateRegistry.Register<EmailConfirmationNotificationTemplate>();
            TemplateRegistry.Register<SimpleTextNotificationTemplate>();
            TemplateRegistry.Register<EmailConfigTemplate>();
            TemplateRegistry.Register<LeagueTemplate>();
            TemplateRegistry.Register<LeaguesConfigTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new LoadUsersSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserLabelSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserLabelLoadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RankIconSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserLabelUidSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ClientLocaleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NotificationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EmailInputValidationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new JoinScreenElementToUserSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RankNameSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserExperienceIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ProfileScreenNavigationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ProfileScreenLoadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ProfileScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ChangeUserEmailScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ConfirmUserEmailScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EmailConfirmationNotificationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ViewUserEmailScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnterUserEmailScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnterConfirmationCodeScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnterNewPasswordScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HelpButtonSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NewsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LeaguesSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TopLeagueInfoSystem());
            TemplateRegistry.RegisterPart<UserLeagueTemplatePart>();
            TemplateRegistry.RegisterPart<GameplayChestUserTemplatePart>();
            TemplateRegistry.Register<FractionTemplate>();
            TemplateRegistry.Register<FractionsCompetitionTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new FractionsCompetitionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new FractionsCompetitionUiSystem());
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

