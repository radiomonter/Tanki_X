namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AlloyToStandardMaterialsMap : MonoBehaviour
    {
        [SerializeField]
        private List<Material> alloyMaterials = new List<Material>();
        [SerializeField]
        private List<Material> standardMaterials = new List<Material>();
        [SerializeField]
        private List<Material> usedMaterials = new List<Material>();

        public void AddStandardReplacement(Material alloy, Material standard)
        {
            this.alloyMaterials.Add(alloy);
            this.standardMaterials.Add(standard);
        }

        public Material GetStandardReplacement(Material alloy)
        {
            int index = this.alloyMaterials.IndexOf(alloy);
            return this.standardMaterials[index];
        }

        public bool HasStandardReplacement(Material alloy) => 
            this.alloyMaterials.Contains(alloy);

        public List<Material> UsedMaterials =>
            this.usedMaterials;

        public List<Material> AlloyMaterials =>
            this.alloyMaterials;

        public List<Material> StandardMaterials =>
            this.standardMaterials;
    }
}

