namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class ModulesUtils
    {
        public static bool EarlyIsUserItem(Entity item) => 
            typeof(UserItemTemplate).IsAssignableFrom(((EntityImpl) item).TemplateAccessor.Get().TemplateDescription.TemplateClass);

        public static Color StringToColor(string s)
        {
            Random random = new Random(s.GetHashCode());
            return new Color((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble());
        }
    }
}

