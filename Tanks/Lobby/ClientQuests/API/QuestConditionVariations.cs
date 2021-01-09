namespace Tanks.Lobby.ClientQuests.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class QuestConditionVariations
    {
        public QuestConditionType Type { get; set; }

        public List<long> items { get; set; }
    }
}

