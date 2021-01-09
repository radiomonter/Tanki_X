namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct MoveCommand
    {
        [ProtocolOptional]
        public Tanks.Battle.ClientCore.Impl.Movement? Movement { get; set; }
        [ProtocolOptional]
        public float? WeaponRotation { get; set; }
        [ProtocolOptional]
        public float TankControlVertical { get; set; }
        [ProtocolOptional]
        public float TankControlHorizontal { get; set; }
        [ProtocolOptional]
        public float WeaponRotationControl { get; set; }
        public int ClientTime { get; set; }
        public override string ToString() => 
            $"MoveCommand[Movement={this.Movement}, WeaponRotation={this.WeaponRotation}]";

        public bool IsDiscrete() => 
            (this.IsFloatDiscrete(this.TankControlVertical) && this.IsFloatDiscrete(this.TankControlHorizontal)) && this.IsFloatDiscrete(this.WeaponRotationControl);

        private bool IsFloatDiscrete(float val) => 
            ((val == 0f) || (val == 1f)) || (val == -1f);
    }
}

