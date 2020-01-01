using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Payments.Responses
{
    public enum Code
    {
        Ok,
        Error,
        CustomerDoesNotExit
    }
    
    public class ServerResponse
    {
        private readonly Dictionary<Code, string> CodesToText = new Dictionary<Code, string>()
        {
            { Responses.Code.Ok, "Ok" },
            { Responses.Code.Error, "Error" },
            { Responses.Code.CustomerDoesNotExit, "CustomerDoesNotExit" },
        };

        private ServerResponse(Code code, object data)
        {
            Code = CodesToText[code];
            Data = EncryptData(JsonConvert.SerializeObject(data));
        }

        public static ServerResponse Ok(object data)
        {
            return new ServerResponse(Responses.Code.Ok, data);
        }

        public static ServerResponse Error(object data)
        {
            return new ServerResponse(Responses.Code.Error, data);
        }

        public static ServerResponse CustomerDoesNotExist(object data)
        {
            return new ServerResponse(Responses.Code.CustomerDoesNotExit, data);
        }

        public string Code { get; set; }
        public object Data { get; set; }

        private byte[] AESEncrypt(ICryptoTransform cryptoTransform, string input)
        {
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, cryptoTransform, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(input);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        private string EncryptData(string input)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(GetPublicKey(), out int bytesRead);

                string result;
                if (input.Length > 400)
                {
                    using (Aes aes = Aes.Create())
                    {
                        var key = aes.Key;

                        var encryptor = aes.CreateEncryptor(key, aes.IV);

                        var obj = new
                        {
                            key = Convert.ToBase64String(rsa.Encrypt(key, RSAEncryptionPadding.OaepSHA1)),
                            iv = Convert.ToBase64String(rsa.Encrypt(aes.IV, RSAEncryptionPadding.OaepSHA1)),
                            data = Convert.ToBase64String(AESEncrypt(encryptor, input))
                        };

                        result = JsonConvert.SerializeObject(obj);
                    }

                }
                else
                {
                    var encryptedInput = rsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.OaepSHA1);
                    result = Convert.ToBase64String(encryptedInput);
                }

                return result;
            }
        }

        private static byte[] GetPublicKey()
        {
            string buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileData = File.ReadAllText(buildDir + @"\Keys\public.pem");
            string key = String.Join("", fileData.Split('\n')[1..^2]);

            return Convert.FromBase64String(key);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
