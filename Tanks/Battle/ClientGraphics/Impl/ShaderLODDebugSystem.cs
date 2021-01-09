namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ShaderLODDebugSystem : ECSSystem
    {
        private static bool simpleShaderMode;

        [OnEventFire]
        public void Update(UpdateEvent e, SingleNode<SelfBattleUserComponent> battle)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                simpleShaderMode = !simpleShaderMode;
            }
            Shader.globalMaximumLOD = !simpleShaderMode ? 500 : 100;
        }
    }
}

