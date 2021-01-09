namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class TeamColorExtensions
    {
        public static TeamColor GetOposite(this TeamColor color) => 
            (color == TeamColor.BLUE) ? TeamColor.RED : ((color == TeamColor.RED) ? TeamColor.BLUE : GetRandom());

        private static TeamColor GetRandom() => 
            (Random.value <= 0.5) ? TeamColor.BLUE : TeamColor.RED;
    }
}

