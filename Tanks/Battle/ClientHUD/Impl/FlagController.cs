namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class FlagController : MonoBehaviour
    {
        private Action onReset;
        private string lastState;

        public void Drop()
        {
            this.SetState("Drop");
        }

        private void OnDisable()
        {
            this.lastState = null;
        }

        private void OnEnable()
        {
            if (this.lastState != null)
            {
                base.GetComponent<Animator>().SetTrigger(this.lastState);
            }
        }

        public void PickUp()
        {
            this.SetState("PickUp");
        }

        private void Reset()
        {
            if (this.onReset != null)
            {
                this.onReset();
            }
            this.onReset = null;
        }

        public void Return(Action onReset)
        {
            this.onReset += onReset;
            this.SetState("Return");
        }

        private void SetState(string state)
        {
            this.lastState = state;
            if (base.gameObject.activeInHierarchy)
            {
                base.GetComponent<Animator>().SetTrigger(state);
            }
        }

        public void TurnIn(Action onReset)
        {
            this.onReset += onReset;
            this.SetState("TurnIn");
        }
    }
}

