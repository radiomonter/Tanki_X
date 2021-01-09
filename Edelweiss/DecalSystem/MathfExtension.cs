namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    internal static class MathfExtension
    {
        public static bool Approximately(float a_Float1, float a_Float2, float a_MaximumAbsoluteError, float a_MaximumRelativeError)
        {
            bool flag = false;
            float num = Mathf.Abs((float) (a_Float1 - a_Float2));
            if (num <= a_MaximumAbsoluteError)
            {
                flag = true;
            }
            else if (num <= (Mathf.Max(Mathf.Abs(a_Float1), Mathf.Abs(a_Float2)) * a_MaximumRelativeError))
            {
                flag = true;
            }
            return flag;
        }
    }
}

