using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Payments.Controllers.AuthentificationController;

namespace Payments.Services.Interfaces
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(RequestTokenModel request, out string token);
    }
}
