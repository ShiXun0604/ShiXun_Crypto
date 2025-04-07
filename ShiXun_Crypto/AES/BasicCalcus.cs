using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiXun_Crypto.AES
{
    internal class BasicCalcus
    {
        private static byte byte_cache1;
        private static byte byte_cache2;
        private static byte byte_cache3;
        private static byte byte_cache4;
        internal static void AddRoundKey(ref byte[] return_data, ref byte[] round_key, int curr_idx)
        {
            for (int i = curr_idx; i < curr_idx+16; i++)
            {
                return_data[i] = (byte)(return_data[i]^round_key[i%16]);
            }
        }
        internal static void SubBytes(ref byte[] return_data, int curr_idx)
        {
            for (int i = curr_idx; i < curr_idx+16; i++)
            {
                return_data[i] = Statics.SBOX[return_data[i]];
            }
        }
        internal static void ShiftRows(ref byte[] return_data, int curr_idx)
        {
            (return_data[curr_idx + 1], return_data[curr_idx+5], return_data[curr_idx + 9], return_data[curr_idx + 13]) = 
                (return_data[curr_idx + 5], return_data[curr_idx + 9], return_data[curr_idx + 13], return_data[curr_idx + 1]);
            (return_data[curr_idx + 2], return_data[curr_idx + 6], return_data[curr_idx + 10], return_data[curr_idx + 14]) 
                = (return_data[curr_idx + 10], return_data[curr_idx + 14], return_data[curr_idx + 2], return_data[curr_idx + 6]);
            (return_data[curr_idx + 3], return_data[curr_idx + 7], return_data[curr_idx + 11], return_data[curr_idx + 15]) 
                = (return_data[curr_idx + 15], return_data[curr_idx + 3], return_data[curr_idx + 7], return_data[curr_idx + 11]);
        }
        internal static void MixColumns(ref byte[] return_data, int curr_idx)
        {
            for (int i = curr_idx; i < curr_idx+16; i += 4)
            {
                byte_cache1 = return_data[i];
                byte_cache2 = return_data[i + 1];
                byte_cache3 = return_data[i + 2];
                byte_cache4 = return_data[i + 3];

                return_data[i] = (byte)(Statics.MultiplyTable[0x01, byte_cache1] ^ Statics.MultiplyTable[0x02, byte_cache2] ^ byte_cache3 ^ byte_cache4);
                return_data[i + 1] = (byte)(byte_cache1 ^ Statics.MultiplyTable[0x01, byte_cache2] ^ Statics.MultiplyTable[0x02, byte_cache3] ^ byte_cache4);
                return_data[i + 2] = (byte)(byte_cache1 ^ byte_cache2 ^ Statics.MultiplyTable[0x01, byte_cache3] ^ Statics.MultiplyTable[0x02, byte_cache4]);
                return_data[i + 3] = (byte)(Statics.MultiplyTable[0x02, byte_cache1] ^ byte_cache2 ^ byte_cache3 ^ Statics.MultiplyTable[0x01, byte_cache4]);
            }
        }
    }
}
