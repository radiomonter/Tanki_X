namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using UnityEngine;

    public static class InputMapping
    {
        private static readonly string HORIZONTAL = "Horizontal";
        private static readonly string VERTICAL = "Vertical";
        private static readonly string CANCEL = "Cancel";
        private static readonly string SUBMIT = "Submit";

        public static float Horizontal =>
            Input.GetAxis(HORIZONTAL);

        public static float Vertical =>
            Input.GetAxis(VERTICAL);

        public static bool Cancel =>
            Input.GetButtonDown(CANCEL);

        public static bool Submit =>
            Input.GetButtonDown(SUBMIT);
    }
}

