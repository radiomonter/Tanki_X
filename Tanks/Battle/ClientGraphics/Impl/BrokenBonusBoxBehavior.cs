namespace Tanks.Battle.ClientGraphics.Impl
{
    using UnityEngine;

    public class BrokenBonusBoxBehavior : MonoBehaviour
    {
        [SerializeField]
        private GameObject brokenBonusGameObject;

        public GameObject BrokenBonusGameObject =>
            this.brokenBonusGameObject;
    }
}

