namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class PhysicsSetup
    {
        private const byte LAYERS_COUNT = 0x20;
        private static bool[] matrix = new bool[0x400];

        private static void AddCollisionCheck(int layer1, params int[] layer2)
        {
            foreach (int num in layer2)
            {
                matrix[GetIndex(layer1, num)] = true;
            }
        }

        private static void Check()
        {
            for (int i = 0; i < 0x20; i++)
            {
                if (!string.IsNullOrEmpty(LayerMask.LayerToName(i)))
                {
                    for (int j = 0; j < 0x20; j++)
                    {
                        if (!string.IsNullOrEmpty(LayerMask.LayerToName(j)))
                        {
                            bool flag = matrix[GetIndex(i, j)] || matrix[GetIndex(j, i)];
                            if (!Physics.GetIgnoreLayerCollision(i, j) != flag)
                            {
                                object[] objArray1 = new object[] { "Collision matrix setup error: shouldCollide=", flag, " layer1=", i, " layer2=", j };
                                throw new Exception(string.Concat(objArray1));
                            }
                        }
                    }
                }
            }
        }

        public static void CheckCollisionMatrix()
        {
            int[] numArray1 = new int[] { Layers.STATIC, Layers.DEFAULT };
            AddCollisionCheck(Layers.DEFAULT, numArray1);
            int[] numArray2 = new int[] { Layers.TANK_TO_STATIC, Layers.STATIC };
            AddCollisionCheck(Layers.STATIC, numArray2);
            int[] numArray3 = new int[] { Layers.REMOTE_TANK_BOUNDS };
            AddCollisionCheck(Layers.SELF_SEMIACTIVE_TANK_BOUNDS, numArray3);
            int[] numArray4 = new int[] { Layers.TANK_TO_TANK };
            AddCollisionCheck(Layers.TANK_TO_TANK, numArray4);
            int[] numArray5 = new int[] { Layers.SELF_TANK_BOUNDS };
            AddCollisionCheck(Layers.TRIGGER_WITH_SELF_TANK, numArray5);
            int[] numArray6 = new int[] { Layers.STATIC, Layers.TANK_TO_TANK, Layers.TANK_AND_STATIC };
            AddCollisionCheck(Layers.TANK_AND_STATIC, numArray6);
            int[] numArray7 = new int[] { Layers.FRICTION, Layers.STATIC };
            AddCollisionCheck(Layers.FRICTION, numArray7);
            int[] numArray8 = new int[] { Layers.MINOR_VISUAL, Layers.STATIC };
            AddCollisionCheck(Layers.MINOR_VISUAL, numArray8);
            Check();
        }

        private static int GetIndex(int i, int j) => 
            (i * 0x20) + j;
    }
}

