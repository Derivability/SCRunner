using System.Text;

namespace SCRunner_Hollow
{
    public static class Shellcode
    {
        static readonly string enc_key = "";
        readonly static byte[] enc_bytes = { };
        static byte[] Decrypt(string key, byte[] buf)
        {
            byte[] result = new byte[buf.Length];
            byte[] key_bytes = Encoding.ASCII.GetBytes(key);

            for (int i = 0; i < buf.Length; i++)
            {
                result[i] = (byte)(buf[i] ^ key_bytes[i % key_bytes.Length]);
            }
            return result;
        }

        public static byte[] bytes = Decrypt(enc_key, enc_bytes);
    }
}
