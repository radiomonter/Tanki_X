namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public static class TankBuilderUtil
    {
        private const string HULL_RENDERER_NAME = "body";
        private const string WEAPON_RENDERER_NAME = "weapon";
        private const string CONTAINER_RENDERER_NAME = "container";

        public static Renderer GetContainerRenderer(GameObject hull) => 
            GraphicsBuilderUtils.GetRenderer(hull.transform.Find("container").gameObject);

        public static Renderer GetHullRenderer(GameObject hull) => 
            GraphicsBuilderUtils.GetRenderer(hull.GetComponentInChildren<TankVisualRootComponent>().transform.Find("body").gameObject);

        public static Renderer GetWeaponRenderer(GameObject weapon) => 
            GraphicsBuilderUtils.GetRenderer(weapon.GetComponentInChildren<WeaponVisualRootComponent>().transform.Find("weapon").gameObject);
    }
}

