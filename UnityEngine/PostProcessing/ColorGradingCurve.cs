namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    [Serializable]
    public sealed class ColorGradingCurve
    {
        public AnimationCurve curve;
        [SerializeField]
        private bool m_Loop;
        [SerializeField]
        private float m_ZeroValue;
        [SerializeField]
        private float m_Range;
        private AnimationCurve m_InternalLoopingCurve;

        public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
        {
            this.curve = curve;
            this.m_ZeroValue = zeroValue;
            this.m_Loop = loop;
            this.m_Range = bounds.magnitude;
        }

        public unsafe void Cache()
        {
            if (this.m_Loop)
            {
                int length = this.curve.length;
                if (length >= 2)
                {
                    this.m_InternalLoopingCurve ??= new AnimationCurve();
                    Keyframe key = this.curve[length - 1];
                    Keyframe* keyframePtr1 = &key;
                    keyframePtr1.time -= this.m_Range;
                    Keyframe keyframe2 = this.curve[0];
                    Keyframe* keyframePtr2 = &keyframe2;
                    keyframePtr2.time += this.m_Range;
                    this.m_InternalLoopingCurve.keys = this.curve.keys;
                    this.m_InternalLoopingCurve.AddKey(key);
                    this.m_InternalLoopingCurve.AddKey(keyframe2);
                }
            }
        }

        public float Evaluate(float t) => 
            (this.curve.length != 0) ? ((!this.m_Loop || (this.curve.length == 1)) ? this.curve.Evaluate(t) : this.m_InternalLoopingCurve.Evaluate(t)) : this.m_ZeroValue;
    }
}

