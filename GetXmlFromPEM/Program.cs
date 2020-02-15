using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace GetXmlFromPEM
{
    class Program
    {
        static void Main(string[] args)
        {
            var passwordArg = args[0];
            var filePathArg = args[1];
            string path = "";
            if (args.Length > 2)
            {
                path = args[2];
            }

            using (RSA rsa = RSA.Create())
            {
                var password = Encoding.UTF8.GetBytes(passwordArg);
                var source = GetPrivateKey(filePathArg);

                rsa.ImportEncryptedPkcs8PrivateKey(password, source, out int bytesRead);
                var xml = rsa.ToXmlString(true);

                File.WriteAllText(Path.Join(path, "private.xml"), xml);
            }
        }

        private static byte[] GetPrivateKey(string path)
        {
            string fileData = File.ReadAllText(path);
            string key = String.Join("", fileData.Split('\n')[1..^2]);

            return Convert.FromBase64String(key);
        }

    }
}
