
using System.Security.Cryptography;
using System.Text;
using Mustache;

namespace VERIDATA.DAL.utility
{
    public static class CommonDalUtility
    {
        public static string hashPassword(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] asByteArray = Encoding.Default.GetBytes(password);
            byte[] hashedPassword = sha.ComputeHash(asByteArray);

            return Convert.ToBase64String(hashedPassword);
        }

        public static string ParseMessage<T1>(string message, T1 payload)
        {
            string content = message;

            if (!string.IsNullOrEmpty(content))
            {
                FormatCompiler compiler = new();
                Generator generator = compiler.Compile(content);

                generator.KeyNotFound += (sender, e) =>
                {
                    e.Substitute = "";
                    e.Handled = true;
                };
                generator.KeyFound += (sender, e) =>
                {
                    e.Substitute = e.Substitute == null ? "" : e.Substitute.ToString() == "True" ?
                        Convert.ToString(e.Substitute).ToLowerInvariant() : e.Substitute;
                };
                generator.ValueRequested += (sender, e) => e.Value ??= "";

                return generator.Render(payload);
            }
            return string.Empty;
        }
        public static string DecryptString(string key, string cipherText)
        {
            string data = string.Empty;
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(string.IsNullOrEmpty(cipherText) ? string.Empty : cipherText);

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                data = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return data;
            }
            return data;
        }

    }

}
