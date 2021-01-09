namespace Tanks.Lobby.ClientQuests.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ConditionDescription
    {
        public string format { get; set; }

        public Dictionary<CaseType, string> cases { get; set; }
    }
}

