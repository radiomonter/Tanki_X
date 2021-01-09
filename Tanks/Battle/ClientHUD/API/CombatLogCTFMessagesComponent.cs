namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CombatLogCTFMessagesComponent : Component
    {
        public static string OWN = "{own}";

        public string BattleStartMessage { get; set; }

        public string Took { get; set; }

        public string Lost { get; set; }

        public string Dropped { get; set; }

        public string Delivered { get; set; }

        public string DeliveryNotCounted { get; set; }

        public string Returned { get; set; }

        public string PickedUp { get; set; }

        public string AutoReturned { get; set; }

        public string BlueFlag { get; set; }

        public string RedFlag { get; set; }

        public string OurFlag { get; set; }

        public string EnemyFlag { get; set; }
    }
}

