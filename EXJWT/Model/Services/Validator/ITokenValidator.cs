using EXJWT.Model.Services.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EXJWT.Model.Services.Validator
{
  public  interface ITokenValidator
    {
        Task Execute(TokenValidatedContext context);
    }

    public class TokenValidator : ITokenValidator
    {
        private readonly UserRepository userRepository;
        public TokenValidator(UserRepository user)
        {
            userRepository = user;

        }
        public async Task Execute(TokenValidatedContext context)
        {
            var claimsidentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsidentity == null)
            {
                context.Fail("claim not fund...");
                return;
            }
            var user = claimsidentity.FindFirst("Id").Value;
            var UserId = int.TryParse(user, out int userId);
            var userFind = userRepository.Get(userId);
            if (!userFind.IsActive)
            {
                context.Fail("user not active ...");
                return;
            }
        }
    }

}
