using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiXun_Crypto.AES
{
    internal class AESKeyCalcu
    {
        internal static byte[] XORByteArrays(byte[] a, byte[] b)
        {
            byte[] result = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }
            return result;
        }
        internal static byte[] SubWord(byte[] a)
        {
            byte[] result = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = Statics.SBOX[a[i]];
            }
            return result;
        }
        internal static byte[] RotWord(byte[] a)
        {
            byte[] result = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = a[(i + 1) % 4];
            }
            return result;
        }
    }

    internal class AESCryptoCalcu
    {
        internal static byte[] AddRoundKey(byte[] a, byte[] b)
        {
            byte[] result = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }
            return result;
        }
        internal static byte[] SubBytes(byte[] a)
        {
            byte[] result = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                result[i] = Statics.SBOX[a[i]];
            }
            return result;
        }
        internal static byte[] InvSubBytes(byte[] a)
        {
            byte[] result = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                result[i] = Statics.INV_SBOX[a[i]];
            }
            return result;
        }
        internal static byte[] ShiftRows(byte[] a)
        {
            byte[] result = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                result[i] = a[Statics.ShiftIndexMap[i]];
            }
            return result;
        }
        internal static byte[] InvShiftRows(byte[] a)
        {
            byte[] result = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                result[i] = a[Statics.InvShiftIndexMap[i]];
            }
            return result;
        }
        internal static byte[] MixColumns(byte[] state)
        {
            byte[] newState = new byte[16];

            for (int i = 0; i < 16; i+=4)
            {
                byte s0 = state[i];
                byte s1 = state[i + 1];
                byte s2 = state[i + 2];
                byte s3 = state[i + 3];

                newState[i] = (byte)(Statics.MultiplyTable[0x01, s0] ^ Statics.MultiplyTable[0x02, s1] ^ s2 ^ s3);
                newState[i + 1] = (byte)(s0 ^ Statics.MultiplyTable[0x01, s1] ^ Statics.MultiplyTable[0x02, s2] ^ s3);
                newState[i + 2] = (byte)(s0 ^ s1 ^ Statics.MultiplyTable[0x01, s2] ^ Statics.MultiplyTable[0x02, s3]);
                newState[i + 3] = (byte)(Statics.MultiplyTable[0x02, s0] ^ s1 ^ s2 ^ Statics.MultiplyTable[0x01, s3]);
            }
            return newState;
        }
        internal static byte[] InvMixColumns(byte[] state)
        {
            byte[] newState = new byte[16];

            for (int i = 0; i < 16; i += 4)
            {
                byte s0 = state[i];
                byte s1 = state[i + 1];
                byte s2 = state[i + 2];
                byte s3 = state[i + 3];

                newState[i] = (byte)(Statics.InvMultiplyTable[0x00, s0] ^ Statics.InvMultiplyTable[0x01, s1] ^ Statics.InvMultiplyTable[0x02, s2] ^ Statics.InvMultiplyTable[0x03, s3]);
                newState[i + 1] = (byte)(Statics.InvMultiplyTable[0x03, s0] ^ Statics.InvMultiplyTable[0x00, s1] ^ Statics.InvMultiplyTable[0x01, s2] ^ Statics.InvMultiplyTable[0x02, s3]);
                newState[i + 2] = (byte)(Statics.InvMultiplyTable[0x02, s0] ^ Statics.InvMultiplyTable[0x03, s1] ^ Statics.InvMultiplyTable[0x00, s2] ^ Statics.InvMultiplyTable[0x01, s3]);
                newState[i + 3] = (byte)(Statics.InvMultiplyTable[0x01, s0] ^ Statics.InvMultiplyTable[0x02, s1] ^ Statics.InvMultiplyTable[0x03, s2] ^ Statics.InvMultiplyTable[0x00, s3]);
            }
            return newState;
        }
        /*
        public static byte Multiply(byte a, byte b)
        {
            if (a == 1)
            {
                return b;
            }

            byte result = 0;
            byte temp = b;

            for (int i = 0; i < 8; i++)
            {
                if ((a & 1) != 0)
                {
                    result ^= temp;
                }

                bool highBitSet = (temp & 0x80) != 0;
                temp <<= 1;

                if (highBitSet)
                {
                    temp ^= 0x1B;
                }

                a >>= 1;
            }

            return result;
        }
        */
    }
}
