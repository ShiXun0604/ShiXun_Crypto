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
        public byte[]? key { get; set; }
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

            this.key = randomData;
            calculate_round_key();

            return this.key;
        }

        // 生成輪金鑰
        private byte[] calculate_round_key() {
            Console.WriteLine("Calculating round key");

            // variable declaration
            int N = this.key_len / 32;
            int R = Statics.ROUNDS[this.mode];
            byte[] round_key = new byte[16 * Statics.ROUNDS[this.mode]];
            Console.WriteLine(N);
            Console.WriteLine(R);

            for (int i = 0; i < 4*R ; i++)
            {
                byte[] K_i = new byte[4];

            }








            this.round_key = round_key;
            return round_key;
        }
    }
}
