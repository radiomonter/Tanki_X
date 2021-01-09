namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using System;

    public class NextLeagueTooltipData
    {
        public double points;
        public int delta;
        public string icon;
        public string name;
        public string unfairMM;

        public NextLeagueTooltipData(double points, string icon, string name, int delta, string unfairMM)
        {
            this.name = name;
            this.points = points;
            this.icon = icon;
            this.delta = delta;
            this.unfairMM = unfairMM;
        }
    }
}

