namespace Tanks.Lobby.ClientGarage.API
{
    using System;

    public class GarageCategory
    {
        public static GarageCategory WEAPONS = new GarageCategory("weapons", typeof(WeaponItemComponent), false);
        public static GarageCategory HULLS = new GarageCategory("hulls", typeof(TankItemComponent), false);
        public static GarageCategory PAINTS = new GarageCategory("paints", typeof(PaintItemComponent), false);
        public static GarageCategory SHELLS = new GarageCategory("shells", typeof(ShellItemComponent), true);
        public static GarageCategory SKINS = new GarageCategory("skins", typeof(SkinItemComponent), true);
        public static GarageCategory GRAFFITI = new GarageCategory("graffiti", typeof(GraffitiItemComponent), false);
        public static GarageCategory BLUEPRINTS = new GarageCategory("blueprints", typeof(GameplayChestItemComponent), false);
        public static GarageCategory CONTAINERS = new GarageCategory("containers", typeof(ContainerMarkerComponent), false);
        public static GarageCategory MODULES = new GarageCategory("modules", typeof(ModuleItemComponent), true);
        public static GarageCategory[] Values;
        private string linkPart;
        private Type itemMarkerComponentType;
        private bool needParent;

        static GarageCategory()
        {
            GarageCategory[] categoryArray1 = new GarageCategory[9];
            categoryArray1[0] = WEAPONS;
            categoryArray1[1] = PAINTS;
            categoryArray1[2] = HULLS;
            categoryArray1[3] = SHELLS;
            categoryArray1[4] = SKINS;
            categoryArray1[5] = GRAFFITI;
            categoryArray1[6] = BLUEPRINTS;
            categoryArray1[7] = CONTAINERS;
            categoryArray1[8] = MODULES;
            Values = categoryArray1;
        }

        public GarageCategory(string linkPart, Type itemMarkerComponentType, bool needParent)
        {
            this.linkPart = linkPart;
            this.itemMarkerComponentType = itemMarkerComponentType;
            this.needParent = needParent;
        }

        public override string ToString() => 
            this.Name;

        public string Name =>
            this.linkPart.ToUpper();

        public string LinkPart =>
            this.linkPart;

        public Type ItemMarkerComponentType =>
            this.itemMarkerComponentType;

        public bool NeedParent =>
            this.needParent;
    }
}

