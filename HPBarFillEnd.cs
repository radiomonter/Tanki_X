using System;
using UnityEngine;

public class HPBarFillEnd : BarFillEnd
{
    [SerializeField]
    private AnimationCurve topCurve;
    [SerializeField]
    private AnimationCurve bottomCurve;

    public override float FillAmount
    {
        set
        {
            base.FillAmount = value;
            base.image.offsetMax = new Vector2(base.image.offsetMax.x, -this.topCurve.Evaluate(value));
            base.image.offsetMin = new Vector2(base.image.offsetMin.x, this.bottomCurve.Evaluate(value));
        }
    }
}

