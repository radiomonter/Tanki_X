namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [SerialVersionUID(0x2f5ec6e59308053fL)]
    public interface IsisBattleItemTemplate : StreamWeaponTemplate, WeaponTemplate, Template
    {
        [PersistentConfig("", false)]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();
        IsisComponent isis();
        [AutoAdded, PersistentConfig("reticle", false)]
        ReticleTemplateComponent reticleTemplate();
    }
}

