namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;

    public class ModuleTooltipData
    {
        public string name;
        public string desc;
        public int upgradeLevel;
        public int maxLevel;
        public List<ModuleVisualProperty> properties;

        public ModuleTooltipData(string name, string desc, int upgradeLevel, int maxLevel, List<ModuleVisualProperty> properties)
        {
            this.name = name;
            this.desc = desc;
            this.upgradeLevel = upgradeLevel;
            this.maxLevel = maxLevel;
            this.properties = properties;
        }
    }
}

