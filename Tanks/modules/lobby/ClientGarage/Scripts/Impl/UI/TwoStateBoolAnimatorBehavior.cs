namespace tanks.modules.lobby.ClientGarage.Scripts.Impl.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class TwoStateBoolAnimatorBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private Animator _targetAnimator;
        [SerializeField]
        private string _boolTriggerName;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if ((this._targetAnimator != null) && !string.IsNullOrEmpty(this._boolTriggerName))
            {
                this._targetAnimator.SetBool(this._boolTriggerName, true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if ((this._targetAnimator != null) && !string.IsNullOrEmpty(this._boolTriggerName))
            {
                this._targetAnimator.SetBool(this._boolTriggerName, false);
            }
        }
    }
}

