using ShiXun_Crypto.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShiXun_Crypto.AES
{
    public class AESCrypto
    {
        public AESKey AESKey { get; set; }
        public AESCrypto(string mode = "AES-128")
        {
            this.AESKey = new AESKey(mode);
        }

        public byte[] encrypt(byte[] data)
        {
            byte[] result = new byte[data.Length];
            int chunk_count = data.Length/16;            

            for (int i = 0; i < chunk_count; i++) 
            {
                // split data into 16 bytes
                byte[] chunk = new byte[16];
                Array.Copy(data, i * 16, chunk, 0, 16);

                // 初始輪
                chunk = AESCryptoCalcu.AddRoundKey(chunk, this.AESKey.round_key[0]);
                //Console.WriteLine(BitConverter.ToString(chunk).Replace("-", " "));

                // 中間輪
                for (int j = 1; j < Statics.ROUNDS[this.AESKey.mode]-1; j++)
                {
                    chunk = AESCryptoCalcu.SubBytes(chunk);
                    chunk = AESCryptoCalcu.ShiftRows(chunk);
                    chunk = AESCryptoCalcu.MixColumns(chunk);
                    chunk = AESCryptoCalcu.AddRoundKey(chunk, this.AESKey.round_key[j]);
                    //Console.WriteLine(BitConverter.ToString(chunk).Replace("-", " "));
                }

                // 最終輪
                chunk = AESCryptoCalcu.SubBytes(chunk);
                chunk = AESCryptoCalcu.ShiftRows(chunk);
                chunk = AESCryptoCalcu.AddRoundKey(chunk, this.AESKey.round_key[Statics.ROUNDS[this.AESKey.mode]-1]);
                //Console.WriteLine(BitConverter.ToString(chunk).Replace("-", " "));
                Array.Copy(chunk, 0, result, i * 16, 16);
            }
            return result;
        }
        public byte[] decrypt(byte[] enc_data)
        {
            byte[] result = new byte[enc_data.Length];
            int chunk_count = enc_data.Length / 16;

            for (int i = 0; i < chunk_count; i++) 
            {
                // split data into 16 bytes
                byte[] chunk = new byte[16];
                Array.Copy(enc_data, i * 16, chunk, 0, 16);

                // 初始輪
                chunk = AESCryptoCalcu.AddRoundKey(chunk, this.AESKey.round_key[Statics.ROUNDS[this.AESKey.mode] - 1]);
                chunk = AESCryptoCalcu.InvShiftRows(chunk);
                chunk = AESCryptoCalcu.InvSubBytes(chunk);
                //Console.WriteLine(BitConverter.ToString(chunk).Replace("-", " "));

                // 中間輪
                for (int j = 1; j < Statics.ROUNDS[this.AESKey.mode] - 1; j++)
                {
                    chunk = AESCryptoCalcu.AddRoundKey(chunk, this.AESKey.round_key[Statics.ROUNDS[this.AESKey.mode]-j-1]);
                    chunk = AESCryptoCalcu.InvMixColumns(chunk);
                    chunk = AESCryptoCalcu.InvShiftRows(chunk);
                    chunk = AESCryptoCalcu.InvSubBytes(chunk);
                    //Console.WriteLine(BitConverter.ToString(chunk).Replace("-", " "));
                }

                // 最終輪
                chunk = AESCryptoCalcu.AddRoundKey(chunk, this.AESKey.round_key[0]);
                Array.Copy(chunk, 0, result, i * 16, 16);
                //Console.WriteLine(BitConverter.ToString(chunk).Replace("-", " "));
            }
            return result;
        }
    }
}
