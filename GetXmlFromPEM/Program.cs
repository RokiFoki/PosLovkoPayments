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
            Console.WriteLine(args);

            var passwordArg = "T0pSecRit010120";
            var filePathArg = "T0pSecRit010120";

            using (RSA rsa = RSA.Create())
            {
                var password = Encoding.UTF8.GetBytes(passwordArg);
                var source = GetPrivateKey(filePathArg);

                rsa.ImportEncryptedPkcs8PrivateKey(password, source, out int bytesRead);
                var xml = rsa.ToXmlString(true);

                File.WriteAllText("private.xml", xml);
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
