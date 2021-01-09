namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections;
    using UnityEngine;

    public class MovementCodec : AbstractMoveCodec
    {
        private static readonly int MOVEMENT_SIZE = 0x15;
        private static readonly int POSITION_COMPONENT_BITSIZE = 0x11;
        private static readonly int ORIENTATION_COMPONENT_BITSIZE = 13;
        private static readonly int LINEAR_VELOCITY_COMPONENT_BITSIZE = 13;
        private static readonly int ANGULAR_VELOCITY_COMPONENT_BITSIZE = 13;
        private static readonly float POSITION_FACTOR = 0.01f;
        private static readonly float VELOCITY_FACTOR = 0.01f;
        private static readonly float ANGULAR_VELOCITY_FACTOR = 0.005f;
        private static readonly float ORIENTATION_PRECISION = (2f / ((float) (1 << (ORIENTATION_COMPONENT_BITSIZE & 0x1f))));

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            Movement movement = new Movement();
            byte[] bytes = new byte[MOVEMENT_SIZE];
            BitArray bits = new BitArray(bytes);
            int position = 0;
            protocolBuffer.Reader.Read(bytes, 0, bytes.Length);
            base.CopyBits(bytes, bits);
            movement.Position = ReadVector3(bits, ref position, POSITION_COMPONENT_BITSIZE, POSITION_FACTOR);
            movement.Orientation = ReadQuaternion(bits, ref position, ORIENTATION_COMPONENT_BITSIZE, ORIENTATION_PRECISION);
            movement.Velocity = ReadVector3(bits, ref position, LINEAR_VELOCITY_COMPONENT_BITSIZE, VELOCITY_FACTOR);
            movement.AngularVelocity = ReadVector3(bits, ref position, ANGULAR_VELOCITY_COMPONENT_BITSIZE, ANGULAR_VELOCITY_FACTOR);
            if (position != bits.Length)
            {
                throw new Exception("Movement unpack mismatch");
            }
            return movement;
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            Movement movement = (Movement) data;
            byte[] bytes = new byte[MOVEMENT_SIZE];
            BitArray bits = new BitArray(bytes);
            int position = 0;
            WriteVector3(bits, ref position, movement.Position, POSITION_COMPONENT_BITSIZE, POSITION_FACTOR);
            WriteQuaternion(bits, ref position, movement.Orientation, ORIENTATION_COMPONENT_BITSIZE, ORIENTATION_PRECISION);
            WriteVector3(bits, ref position, movement.Velocity, LINEAR_VELOCITY_COMPONENT_BITSIZE, VELOCITY_FACTOR);
            WriteVector3(bits, ref position, movement.AngularVelocity, ANGULAR_VELOCITY_COMPONENT_BITSIZE, ANGULAR_VELOCITY_FACTOR);
            bits.CopyTo(bytes, 0);
            protocolBuffer.Writer.Write(bytes);
            if (position != bits.Length)
            {
                throw new Exception("Movement pack mismatch");
            }
        }

        public override void Init(Protocol protocol)
        {
        }

        private static Quaternion ReadQuaternion(BitArray bits, ref int position, int size, float factor)
        {
            Quaternion quaternion = new Quaternion {
                x = ReadFloat(bits, ref position, size, factor),
                y = ReadFloat(bits, ref position, size, factor),
                z = ReadFloat(bits, ref position, size, factor)
            };
            quaternion.w = Mathf.Sqrt(1f - (((quaternion.x * quaternion.x) + (quaternion.y * quaternion.y)) + (quaternion.z * quaternion.z)));
            if (double.IsNaN((double) quaternion.w))
            {
                quaternion.w = 0f;
            }
            return quaternion;
        }

        private static Vector3 ReadVector3(BitArray bits, ref int position, int size, float factor) => 
            new Vector3 { 
                x = ReadFloat(bits, ref position, size, factor),
                y = ReadFloat(bits, ref position, size, factor),
                z = ReadFloat(bits, ref position, size, factor)
            };

        private static void WriteQuaternion(BitArray bits, ref int position, Quaternion value, int size, float factor)
        {
            int num = (value.w < 0f) ? -1 : 1;
            WriteFloat(bits, ref position, value.x * num, size, factor);
            WriteFloat(bits, ref position, value.y * num, size, factor);
            WriteFloat(bits, ref position, value.z * num, size, factor);
        }

        private static void WriteVector3(BitArray bits, ref int position, Vector3 value, int size, float factor)
        {
            WriteFloat(bits, ref position, value.x, size, factor);
            WriteFloat(bits, ref position, value.y, size, factor);
            WriteFloat(bits, ref position, value.z, size, factor);
        }
    }
}

