/*
 * by uranusq https://github.com/NL0bP/aaa_emulator
 *
 * by NLObP: метод шифрации для ВСЕХ
 */

using System.Security.Cryptography;

namespace AAEmu.Commons.Cryptography
{
    public class CryptRSA
    {
        public byte[] PubKey { get; set; }
        public byte[] PrivKey { get; set; }
        public byte[] Modulus { get; set; }
        public byte[] Exponent { get; set; }
        public byte[] AesKey { get; set; }
        public byte[] Iv { get; set; }
        public uint XorKey { get; set; }
        public RSACryptoServiceProvider Rsa { get; set; }

        public CryptRSA()
        {
            Rsa = new RSACryptoServiceProvider(1024);
            PubKey = Rsa.ExportCspBlob(false); //сохраним открытый ключ в byte[]
            PrivKey = Rsa.ExportCspBlob(true); //сохраним приватный ключ в byte[]
            AesKey = new byte[16];
            Iv = new byte[16];
            XorKey = 0;
        }

        /// <summary>
        /// GetAesKey ... extracts AES key
        /// </summary>
        public void GetAesKey(byte[] raw)
        {
            Rsa.ImportCspBlob(PrivKey); //получить приватный ключ
            AesKey = Rsa.Decrypt(raw, false); //расшифровать с секретным ключом
        }

        /// <summary>
        /// GetXorKey ... extracts XOR key
        /// </summary>
        public void GetXorKey(byte[] raw)
        {
            Rsa.ImportCspBlob(PrivKey); //получить секретный ключ
            var keyXoRraw = Rsa.Decrypt(raw, false); //расшифровать с секретным ключом

            var head = System.BitConverter.ToUInt32(keyXoRraw, 0);
            XorKey = (head ^ 0x15a0248e) * head ^ 0x070f1f23 & 0xffffffff; // 3.0.3.0 archerage.to
        }
        public void GetNevIv()
        {
            Iv = new byte[16];
        }
    }
}
