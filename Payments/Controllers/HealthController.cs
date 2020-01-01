using System;
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
    public class HealthController : ControllerBase
    {
        public HealthController()
        {
        }

        [AllowAnonymous]
        public String Get()
        {
            return "Healthy";
        }
        
    }
}