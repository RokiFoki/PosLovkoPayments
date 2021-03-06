﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payments.Helpers.ModelBinders;
using Payments.Responses;
using Payments.Services.Interfaces;

namespace Payments.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        IAuthenticationService _service;
        public AuthentificationController(IAuthenticationService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public ServerResponse RequestToken(RequestTokenModel request)
        {
            if (!ModelState.IsValid)
            {
                return ServerResponse.Error(null);
            }

            string token;
            if (_service.IsAuthenticated(request, out token))
            {
                return ServerResponse.Ok(token);
            }

            return ServerResponse.Error(null);
        }

        [ModelBinder(BinderType = typeof(DecryptBodyModelBinder<RequestTokenModel>))]
        public class RequestTokenModel
        {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }
    }
}