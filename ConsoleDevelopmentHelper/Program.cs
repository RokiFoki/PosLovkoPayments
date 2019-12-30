﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Payments.Services;
using Payments.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleDevelopmentHelper
{
    class Program
    {

        private static byte[] GetPublicKey()
        {
            string buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileData = File.ReadAllText(buildDir + @"public.pem");
            string key = String.Join("", fileData.Split('\n')[1..^2]);
            
            return Convert.FromBase64String(key);
        }
        
        private static string generateString(int length)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++) sb.Append('b');

            return sb.ToString();
        }

        static void Main(string[] args)
        {
            var input = "1234567890";
            byte[] encryptedInput;
            byte[] decryptedInput;

            for (int i = 1; i < 2000; i++)
            {
                input = generateString(i);

                Console.WriteLine(i);

                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportSubjectPublicKeyInfo(GetPublicKey(), out int bytesRead);

                    Console.WriteLine(Encoding.UTF8.GetBytes(input).Length);
                    Console.WriteLine(GetPublicKey().Length);
                    Console.WriteLine(" ");

                    encryptedInput = rsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.OaepSHA1);
                }
            }
        }
    }
}
