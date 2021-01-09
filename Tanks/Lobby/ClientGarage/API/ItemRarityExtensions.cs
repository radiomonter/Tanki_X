namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class ItemRarityExtensions
    {
        private static readonly Color COMMON_COLOR = new Color(0.86f, 0.86f, 0.86f);
        private static readonly Color RARE_COLOR = new Color(0.24f, 0.72f, 0.97f);
        private static readonly Color EPIC_COLOR = new Color(0.71f, 0.57f, 1f);
        private static readonly Color LEGENDARY_COLOR = new Color(1f, 0.42f, 0.22f);

        public static Color GetRarityColor(this ItemRarityType rarity)
        {
            switch (rarity)
            {
                case ItemRarityType.COMMON:
                    return COMMON_COLOR;

                case ItemRarityType.RARE:
                    return RARE_COLOR;

                case ItemRarityType.EPIC:
                    return EPIC_COLOR;

                case ItemRarityType.LEGENDARY:
                    return LEGENDARY_COLOR;
            }
            return COMMON_COLOR;
        }
    }
}

