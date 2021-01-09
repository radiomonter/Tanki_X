namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System.Collections.Generic;

    public class SquadsRegisterComponent : BehaviourComponent
    {
        public Dictionary<long, Color> squads = new Dictionary<long, Color>();
    }
}

