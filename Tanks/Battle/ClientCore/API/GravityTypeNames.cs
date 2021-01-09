namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GravityTypeNames
    {
        public const string configPath = "localization/gravity_type";

        public Dictionary<GravityType, string> Names { get; set; }
    }
}

