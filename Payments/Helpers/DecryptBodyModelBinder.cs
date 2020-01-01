﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Helpers.ModelBinders
{
    public class DecryptBodyModelBinder<T> : IModelBinder
    {
        private const string ModelDataName = "data";

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            
            var valueProviderResult =
                bindingContext.ValueProvider.GetValue(ModelDataName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName,
                valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(JsonConvert.DeserializeObject<T>(DecryptData(value)));

            return Task.CompletedTask;
        }

        private string DecryptData(string data)
        {
            using (RSA rsa = RSA.Create())
            {
                var password = Convert.FromBase64String("");
                var source = GetPrivateKey();
                try
                {
                    rsa.ImportEncryptedPkcs8PrivateKey(password, source, out int bytesRead);
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                var decryptedInput = rsa.Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.OaepSHA1);

                return Encoding.UTF8.GetString(decryptedInput);
            }
        }

        private static byte[] GetPrivateKey()
        {
            string buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileData = File.ReadAllText(buildDir + @"\Keys\private.pem");
            string key = String.Join("", fileData.Split('\n')[1..^2]);

            return Convert.FromBase64String(key);
        }
    }
}
