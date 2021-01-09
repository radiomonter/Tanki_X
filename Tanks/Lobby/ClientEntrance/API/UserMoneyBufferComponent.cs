namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class UserMoneyBufferComponent : Component
    {
        private int crystalBuffer;
        private int xCrystalBuffer;

        public void ChangeCrystalBufferBy(int delta)
        {
            this.crystalBuffer += delta;
        }

        public void ChangeXCrystalBufferBy(int delta)
        {
            this.xCrystalBuffer += delta;
        }

        public int CrystalBuffer
        {
            get => 
                this.crystalBuffer;
            set => 
                this.crystalBuffer = value;
        }

        public int XCrystalBuffer
        {
            get => 
                this.xCrystalBuffer;
            set => 
                this.xCrystalBuffer = value;
        }
    }
}

