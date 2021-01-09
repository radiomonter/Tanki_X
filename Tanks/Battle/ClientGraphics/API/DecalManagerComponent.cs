namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class DecalManagerComponent : Component
    {
        private LinkedList<DecalEntry> decalsQueue = new LinkedList<DecalEntry>();
        private Tanks.Battle.ClientGraphics.API.DecalMeshBuilder _decalMeshBuilder;
        private Tanks.Battle.ClientGraphics.API.BulletHoleDecalManager bulletHoleDecalManager;
        private Tanks.Battle.ClientGraphics.API.GraffitiDynamicDecalManager graffitiDynamicDecalManager;
        private bool enableDecals;

        public LinkedList<DecalEntry> DecalsQueue =>
            this.decalsQueue;

        public Tanks.Battle.ClientGraphics.API.BulletHoleDecalManager BulletHoleDecalManager
        {
            get => 
                this.bulletHoleDecalManager;
            set => 
                this.bulletHoleDecalManager = value;
        }

        public Tanks.Battle.ClientGraphics.API.GraffitiDynamicDecalManager GraffitiDynamicDecalManager
        {
            get => 
                this.graffitiDynamicDecalManager;
            set => 
                this.graffitiDynamicDecalManager = value;
        }

        public Tanks.Battle.ClientGraphics.API.DecalMeshBuilder DecalMeshBuilder
        {
            get => 
                this._decalMeshBuilder;
            set => 
                this._decalMeshBuilder = value;
        }

        public bool EnableDecals
        {
            get => 
                this.enableDecals;
            set => 
                this.enableDecals = value;
        }
    }
}

