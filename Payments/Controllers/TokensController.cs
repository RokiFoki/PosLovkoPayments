﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payments.Responses;
using Payments.Services.Interfaces;

namespace Payments.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        IBraintreeService braintreeService;

        public TokensController(IBraintreeService braintreeService)
        {
            this.braintreeService = braintreeService;
        }

        [HttpGet("{id}")]
        public ServerResponse Get(string id)
        {
            try
            {
                return ServerResponse.Ok(braintreeService.GenerateClientToken(id));
            }
            catch (ArgumentException)
            {
                return ServerResponse.CustomerDoesNotExist("");
            }
        }
    }
}