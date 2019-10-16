using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Payments.Services;
using Payments.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleDevelopmentHelper
{
    class Program
    {

        private static byte[] GetPublicKey()
        {
            string fileData = System.IO.File.ReadAllText(@"C:\Users\rokok\source\repos\Payments\Payments\Keys\public3.pem");
            string key = String.Join("", fileData.Split('\n')[1..^2]);
            
            return Convert.FromBase64String(key);
        }

        private static byte[] GetPrivateKey()
        {
            string fileData = System.IO.File.ReadAllText(@"C:\Users\rokok\source\repos\Payments\Payments\Keys\private3.pem");
            string key = String.Join("", fileData.Split('\n')[1..^2]);
            
            return Convert.FromBase64String(key);
        }

        static void Main(string[] args)
        {
            var input = "Hello world";
            byte[] encryptedInput;
            byte[] decryptedInput;

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(GetPublicKey(), out int bytesRead);

                encryptedInput = rsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.OaepSHA1);
            }

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportEncryptedPkcs8PrivateKey(Convert.FromBase64String(""), GetPrivateKey(), out int bytesRead);

                decryptedInput = rsa.Decrypt(encryptedInput, RSAEncryptionPadding.OaepSHA1);

                Console.WriteLine(Encoding.UTF8.GetString(decryptedInput));
            }
        }
    }
}
