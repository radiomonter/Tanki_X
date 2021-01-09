namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Threading;
    using UnityEngine;

    public class FreezeGeneratorSystem : ECSSystem
    {
        [OnEventFire]
        public void GenerateFreeze(UpdateEvent e, SingleNode<UserReadyToBattleComponent> any)
        {
            if (Random.Range((float) 0f, (float) 1f) < 0.03f)
            {
                Thread.Sleep(Random.Range(0, 500));
            }
        }
    }
}

