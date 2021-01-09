namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer;

    public class ClientBattleSelectActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.RegisterPart<BattleTemplatePart>();
            TemplateRegistry.Register<BattleSelectTemplate>();
            TemplateRegistry.Register<BattleSelectScreenTemplate>();
            TemplateRegistry.Register<BattleLobbyTemplate>();
            TemplateRegistry.Register<CustomBattleLobbyTemplate>();
            TemplateRegistry.Register<BattleLobbyChatTemplate>();
            TemplateRegistry.Register<SquadTemplate>();
            TemplateRegistry.Register<BattleResultRewardTemplate>();
            TemplateRegistry.Register<XCrystalBattleRewardTemplate>();
            TemplateRegistry.Register<ModuleContainerBattleRewardTemplate>();
            TemplateRegistry.Register<LeagueFirstEntranceRewardTemplate>();
            TemplateRegistry.Register<TutorialBattleRewardTemplate>();
            TemplateRegistry.Register<LevelUpUnlockBattleRewardTemplate>();
            TemplateRegistry.Register<PersonalBattleRewardTemplate>();
            TemplateRegistry.Register<XCrystalBonusPersonalBattleRewardTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new BattleSelectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserReadyToBattleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CreateCustomBattleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleLobbyScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleLobbyChatSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InviteToLobbySystem());
            ECSBehaviour.EngineService.RegisterSystem(new ConnectToLobbyUiSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CommonScoreTableSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DMScoreTableSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TeamScoreTableSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableEmptyRowsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableUserLabelIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableDeathsIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableKillsIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableScoreIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableEmptyRowIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShowIndicatorOnRoundRestartSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableHullIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableTurretIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTableFlagIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScoreTablePingIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RoundUserEquipmentSystem());
            ECSBehaviour.EngineService.RegisterSystem(new JoinToSelectedBattleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new JoinToScreenBattleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleTimeIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleScoreLimitIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TeamBattleScoreIndicatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleInfoSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleInfoEntranceSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnterAsSpectatorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleDetailsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleInfoTeamViewSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleDetailsDMSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleDetailsTDMSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleSelectLoadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleSelectInviteFriendsScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleSelectInviteFriendsListSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InviteFriendToBattleNotificationSystem());
            TemplateRegistry.Register<InviteFriendToBattleNotificationTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new ReturnToBattleSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MatchLobbyGUISystem());
            ECSBehaviour.EngineService.RegisterSystem(new MatchLobbySoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ECSDumperSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserInteractionsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SquadInfoSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SquadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new InviteToSquadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RequestToSquadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SquadInteractionSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LeaveSquadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SquadColorSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleResultCommonScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleResultAwardsScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleResultStatScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleSeriesUiSystem());
            ECSBehaviour.EngineService.RegisterSystem(new XCrystalBonusRewardsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ModuleContainerRewardSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LevelUpUnlockRewardSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleCoverSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleResultsScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SpecialOfferUiSystem());
            MapRegistrySystem system = new MapRegistrySystem();
            ECSBehaviour.EngineService.RegisterSystem(system);
            ServiceRegistry.Current.RegisterService<MapRegistry>(system);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

