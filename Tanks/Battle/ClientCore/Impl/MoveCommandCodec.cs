namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections;

    public class MoveCommandCodec : AbstractMoveCodec
    {
        private static readonly int WEAPON_ROTATION_SIZE = 2;
        private static readonly int WEAPON_ROTATION_COMPONENT_BITSIZE = (WEAPON_ROTATION_SIZE * 8);
        private static readonly float WEAPON_ROTATION_FACTOR = (360f / ((float) (1 << (WEAPON_ROTATION_COMPONENT_BITSIZE & 0x1f))));
        private static byte[] bufferForWeaponRotation = new byte[WEAPON_ROTATION_SIZE];
        private static byte[] bufferEmpty = new byte[0];
        private static BitArray bitsForWeaponRotation = new BitArray(bufferForWeaponRotation);
        private static BitArray bitsEmpty = new BitArray(bufferEmpty);
        private Codec movementCodec;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            bool flag = protocolBuffer.OptionalMap.Get();
            bool hasWeaponRotation = protocolBuffer.OptionalMap.Get();
            bool flag3 = protocolBuffer.OptionalMap.Get();
            MoveCommand command = new MoveCommand();
            if (!flag3)
            {
                command.TankControlVertical = protocolBuffer.Reader.ReadSingle();
                command.TankControlHorizontal = protocolBuffer.Reader.ReadSingle();
                command.WeaponRotationControl = protocolBuffer.Reader.ReadSingle();
            }
            else
            {
                DiscreteTankControl control = new DiscreteTankControl {
                    Control = protocolBuffer.Reader.ReadByte()
                };
                command.TankControlHorizontal = control.TurnAxis;
                command.TankControlVertical = control.MoveAxis;
                command.WeaponRotationControl = control.WeaponControl;
            }
            if (flag)
            {
                command.Movement = new Movement?((Movement) this.movementCodec.Decode(protocolBuffer));
            }
            byte[] buffer = this.GetBuffer(hasWeaponRotation);
            BitArray bits = this.GetBits(hasWeaponRotation);
            int position = 0;
            protocolBuffer.Reader.Read(buffer, 0, buffer.Length);
            base.CopyBits(buffer, bits);
            if (hasWeaponRotation)
            {
                command.WeaponRotation = new float?(ReadFloat(bits, ref position, WEAPON_ROTATION_COMPONENT_BITSIZE, WEAPON_ROTATION_FACTOR));
            }
            if (position != bits.Length)
            {
                throw new Exception("Move command unpack mismatch");
            }
            command.ClientTime = protocolBuffer.Reader.ReadInt32();
            return command;
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            MoveCommand command = (MoveCommand) data;
            bool optional = command.Movement != null;
            bool flag2 = command.WeaponRotation != null;
            bool flag3 = command.IsDiscrete();
            protocolBuffer.OptionalMap.Add(optional);
            protocolBuffer.OptionalMap.Add(flag2);
            protocolBuffer.OptionalMap.Add(flag3);
            if (!flag3)
            {
                protocolBuffer.Writer.Write(command.TankControlVertical);
                protocolBuffer.Writer.Write(command.TankControlHorizontal);
                protocolBuffer.Writer.Write(command.WeaponRotationControl);
            }
            else
            {
                DiscreteTankControl control = new DiscreteTankControl {
                    MoveAxis = (int) command.TankControlVertical,
                    TurnAxis = (int) command.TankControlHorizontal,
                    WeaponControl = (int) command.WeaponRotationControl
                };
                protocolBuffer.Writer.Write(control.Control);
            }
            if (optional)
            {
                this.movementCodec.Encode(protocolBuffer, command.Movement.Value);
            }
            if (flag2)
            {
                byte[] buffer = this.GetBuffer(flag2);
                BitArray bits = this.GetBits(flag2);
                int position = 0;
                WriteFloat(bits, ref position, command.WeaponRotation.Value, WEAPON_ROTATION_COMPONENT_BITSIZE, WEAPON_ROTATION_FACTOR);
                bits.CopyTo(buffer, 0);
                protocolBuffer.Writer.Write(buffer);
                if (position != bits.Length)
                {
                    throw new Exception("Move command pack mismatch");
                }
            }
            protocolBuffer.Writer.Write(command.ClientTime);
        }

        private BitArray GetBits(bool hasWeaponRotation) => 
            !hasWeaponRotation ? bitsEmpty : bitsForWeaponRotation;

        private byte[] GetBuffer(bool hasWeaponRotation) => 
            !hasWeaponRotation ? bufferEmpty : bufferForWeaponRotation;

        public override void Init(Protocol protocol)
        {
            this.movementCodec = protocol.GetCodec(typeof(Movement));
        }
    }
}

