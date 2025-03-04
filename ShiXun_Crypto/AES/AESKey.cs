using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ShiXun_Crypto.Math;



namespace ShiXun_Crypto.AES
{    
    public class AESKey
    {
        public string mode { get; set; }
        public byte[]? key{ get; set; }
        public byte[][]? round_key { get; set; }
        public int key_len 
        { 
            get 
            {
                return Statics.MODE[this.mode];
            }
        }

        public AESKey(string mode="AES-128")
        {
            this.mode = mode;
        }

        public byte[] generate_key()
        {
            byte[] randomData = new byte[this.key_len/8]; 
            RandomNumberGenerator.Fill(randomData);

            this.key = randomData;
            calculate_round_key();

            return this.key;
        }

        private byte[][] calculate_round_key() {
            // variable declaration
            int N = this.key_len / 32;
            int R = Statics.ROUNDS[this.mode];

            // key schedule https://en.wikipedia.org/wiki/AES_key_schedule
            byte[][] W = new byte[4 * R][];
            byte[][] round_key = new byte[Statics.ROUNDS[this.mode]][];
            for (int i = 0; i < 4*R ; i++)
            {
                if (i < N)
                {
                    byte[] K_i = new byte[4];
                    Array.Copy(this.key, i * 4, K_i, 0, 4);
                    W[i] = K_i;
                }
                else if ( i % N == 0)
                {                       
                    byte[] a = W[i - N];  // a                    
                    byte[] b = AESKeyCalcu.SubWord(AESKeyCalcu.RotWord(W[i - 1]));  // b                    
                    byte[] c = Statics.RCON[i / N];  // c
                    W[i] = AESKeyCalcu.XORByteArrays(AESKeyCalcu.XORByteArrays(a, b), c);
                }
                else if (N > 6 && i % N == 4)
                {                 
                    W[i] = AESKeyCalcu.XORByteArrays(W[i-N], AESKeyCalcu.SubWord(W[i-1]));
                }
                else
                {
                    W[i] = AESKeyCalcu.XORByteArrays(W[i-N], W[i-1]);
                }
                //Console.WriteLine(i + " : " + BitConverter.ToString(W[i]));
                
            }

            // round key
            for (int i = 0; i < R; i++)
            {
                byte[] round_key_i = new byte[16];
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        round_key_i[j * 4 + k] = W[i*4 + j][k];
                    }
                }
                round_key[i] = round_key_i;
                //Console.WriteLine(i + " : " + BitConverter.ToString(round_key_i));
            }
            this.round_key = round_key;
            return round_key;
        }

        public void import_byte_key(byte[] key)
        {
            if (key.Length != this.key_len / 8)
            {
                throw new ArgumentException("Key length must be " + this.key_len / 8 + " bytes");
            }
            else
            {
                this.key = key;
                calculate_round_key();
            }
        }
    }
}