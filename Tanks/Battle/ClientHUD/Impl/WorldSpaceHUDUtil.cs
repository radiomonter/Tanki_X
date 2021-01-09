namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    public class WorldSpaceHUDUtil
    {
        public static void ScaleToRealSize(Transform canvasTransform, Transform elementTransform, Camera camera)
        {
            float num = ((float) camera.pixelHeight) / (2f * Mathf.Tan(0.008726646f * camera.fieldOfView));
            Vector3 vector = camera.WorldToViewportPoint(elementTransform.position);
            float num2 = 1f / canvasTransform.localScale.x;
            float x = (vector.z / num) * num2;
            elementTransform.localScale = new Vector3(x, x, x);
        }
    }
}

