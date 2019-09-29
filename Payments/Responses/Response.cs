using System;
using System.Collections.Generic;

namespace Payments.Responses
{
    public enum Code
    {
        Ok,
        CustomerDoesNotExit
    }

    
    public class Response<T>
    {
        private readonly Dictionary<Code, String> CodesToText = new Dictionary<Code, string>()
        {
            { Responses.Code.Ok, "Ok" },
            { Responses.Code.CustomerDoesNotExit, "CustomerDoesNotExit" },
        };

        private Response(Code code, T data)
        {
            Code = CodesToText[code];
            Data = data;
        }

        public static Response<T> Ok(T data)
        {
            return new Response<T>(Responses.Code.Ok, data);
        }

        public static Response<T> CustomerDoesNotExist(T data)
        {
            return new Response<T>(Responses.Code.CustomerDoesNotExit, data);
        }

        public string Code { get; set; }
        public T Data { get; set; }
    }
}
