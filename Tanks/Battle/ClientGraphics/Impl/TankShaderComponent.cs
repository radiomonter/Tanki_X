namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class TankShaderComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Shader opaqueShader;
        [SerializeField]
        private Shader transparentShader;

        public Shader OpaqueShader =>
            this.opaqueShader;

        public Shader TransparentShader =>
            this.transparentShader;
    }
}

