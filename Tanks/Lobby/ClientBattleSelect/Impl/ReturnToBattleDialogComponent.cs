namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ReturnToBattleDialogComponent : BehaviourComponent
    {
        public int SecondsLeft { get; set; }

        public string PreformatedMainText { get; set; }
    }
}

