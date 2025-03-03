using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShiXun_Crypto.AES
{    
    public class AESKey
    {
        public string mode { get; set; }
        public byte[]? key{ get; set; }
        public byte[]? round_key { get; set; }
        public int key_len 
        { 
            get 
            {
                return Statics.MODE[this.mode];
            } 
        }

        // 建構子
        public AESKey(string mode="AES-128")
        {
            this.mode = mode;
        }

        // 生成金鑰
        public byte[] generate_key()
        {
            // 創建一個 128 位元的隨機數據
            byte[] randomData = new byte[this.key_len/8]; 
            RandomNumberGenerator.Fill(randomData);

            // 寫進class
            this.key = randomData;
            calculate_round_key();

            return this.key;
        }

        // 生成roumd key
        private byte[] calculate_round_key() {
            Console.WriteLine("Calculating round key");

            // variable declaration
            int N = this.key_len / 32;
            int R = Statics.ROUNDS[this.mode];
            
            byte[][] W = new byte[4 * R][];
            for (int i = 0; i < 4*R ; i++)
            {
                if (i < N)
                {
                    byte[] K_i = new byte[4];
                    Array.Copy(this.key, i * 4, K_i, 0, 4);                                      
                    W[i] = K_i;
                    // Console.WriteLine(i + " : " + BitConverter.ToString(W_i[i]));
                }
                else if ( i % N == 0)
                {   
                    // a
                    byte[] a = W[i - N];
                    // b
                    byte[] R_W = new byte[4] { W[i-1][1], W[i-1][2], W[i-1][3], W[i-1][0]};
                    byte[] b = new byte[4] { Statics.SBOX[R_W[0]], Statics.SBOX[R_W[1]], Statics.SBOX[R_W[2]], Statics.SBOX[R_W[3]] };
                    // c
                    byte[] c = Statics.RCON[i / N];

                    W[i] = XORByteArrays(a, XORByteArrays(b, c));
                }
                else
                {
                    W[i] = XORByteArrays(W[i-N], W[i-1]);
                }
                Console.WriteLine(i + " : " + BitConverter.ToString(W[i]));
            }

            
            


            byte[] round_key = new byte[16 * Statics.ROUNDS[this.mode]];


            this.round_key = round_key;
            return round_key;
        }
        static byte[] XORByteArrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Arrays must have the same length");

            byte[] result = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = (byte)(a[i] ^ b[i]);
            }
            return result;
        }
    }
}
