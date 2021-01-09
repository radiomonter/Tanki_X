namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using UnityEngine;

    public class ECSDumperSystem : ECSSystem
    {
        private static KeyCode DUMP_KEY = KeyCode.F12;

        [OnEventFire]
        public void Dump(UpdateEvent e, SingleNode<ClientSessionComponent> session)
        {
            if (Input.GetKeyUp(DUMP_KEY))
            {
                Console.WriteLine("---------------Begin ECSDump--------------");
                Console.WriteLine(ECSStringDumper.Build(true));
                Console.WriteLine("----------------End ECSDump---------------");
            }
        }
    }
}

