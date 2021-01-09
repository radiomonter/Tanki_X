namespace Assets
{
    using System;
    using UnityEngine;

    public class Equip : MonoBehaviour
    {
        private static Equip equipped;

        public void Cancel()
        {
            base.GetComponent<Animator>().SetBool("disassembled", false);
        }

        public void Claim()
        {
            base.GetComponent<Animator>().SetTrigger("claim");
        }

        public void Dis()
        {
            base.GetComponent<Animator>().SetBool("disassembled", true);
        }

        public void Do()
        {
            Animator component = base.GetComponent<Animator>();
            if (!component.GetBool("equipped") && ((equipped != null) && (equipped != this)))
            {
                equipped.GetComponent<Animator>().SetBool("equipped", false);
            }
            component.SetBool("equipped", !component.GetBool("equipped"));
            if (component.GetBool("equipped"))
            {
                equipped = this;
            }
        }

        public void OnClaim()
        {
            Destroy(base.gameObject);
        }
    }
}

