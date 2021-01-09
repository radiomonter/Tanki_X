namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15263404159L)]
    public interface CameraTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        CameraOffsetConfigComponent cameraOffsetConfig();
        [AutoAdded, PersistentConfig("", false)]
        TankCameraShakerConfigOnDeathComponent tankCameraShakerConfigOnDeath();
        [AutoAdded, PersistentConfig("", false)]
        TankFallingCameraShakerConfigComponent tankFallingCameraShakerConfig();
    }
}

