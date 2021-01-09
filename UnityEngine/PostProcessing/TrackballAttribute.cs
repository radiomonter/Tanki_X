namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public sealed class TrackballAttribute : PropertyAttribute
    {
        public readonly string method;

        public TrackballAttribute(string method)
        {
            this.method = method;
        }
    }
}

