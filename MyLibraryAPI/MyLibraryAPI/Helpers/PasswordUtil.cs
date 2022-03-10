using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace MyLibraryAPI.Helpers
{
    public class PasswordUtil
    {
        private readonly Random random;

        public PasswordUtil()
        {
            random = new Random();
        }
        public string PasswordEncrypt(string cleanPassword)
        {
            string cfgVal = ConfigurationManager.AppSettings["HashIterates"];
            int iterates = random.Next(int.Parse(cfgVal ?? "80000"), 999999);
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var df2 = new Rfc2898DeriveBytes(cleanPassword, salt, iterates, HashAlgorithmName.SHA256);
            byte[] hash = df2.GetBytes(38);
            byte[] hashByte = new byte[54];
            Array.Copy(salt, 0, hashByte, 0, 16);
            Array.Copy(hash, 0, hashByte, 16, 34);
            string tmpBase64 = Convert.ToBase64String(hashByte);
            tmpBase64 = "AB"+ iterates.ToString() + "C#" + tmpBase64;
            df2.Dispose();
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(tmpBase64));
        }

        public static bool PasswordVerify(string cleanPassword, string hashedPassword)
        {
            string composite = Encoding.UTF8.GetString(Convert.FromBase64String(hashedPassword));
            int delimiter = composite.IndexOf("C#");
            int iterates = Convert.ToInt32(composite.Substring(2, delimiter - 1));
            string b64 = composite.Substring(delimiter + 2, composite.Length - (delimiter + 2));
            byte[] hashed = Convert.FromBase64String(b64);
            byte[] salt = new byte[16];
            Array.Copy(hashed, 0, salt, 0, 16);
            var df2 = new Rfc2898DeriveBytes(cleanPassword, salt, iterates, HashAlgorithmName.SHA256);
            byte[] hash = df2.GetBytes(38);
            df2.Dispose();
            for (int i = 0; i < 38; i++)
            {
                if (hashed[i + 16] != hash[i])
                    return false;
            }
            return true;
        }
    }
}