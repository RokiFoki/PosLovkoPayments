using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.IdentityModel.Tokens;
using Payments.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Payments.Controllers.AuthentificationController;

namespace Payments.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public IConfiguration Configuration { get; }

        public AuthenticationService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool IsAuthenticated(RequestTokenModel request, out string token)
        {
            token = String.Empty;
            if (request.Password != Configuration["ApiKey"]) return false;

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, request.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                null,
                null,
                claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            return true;
        }
    }
}
