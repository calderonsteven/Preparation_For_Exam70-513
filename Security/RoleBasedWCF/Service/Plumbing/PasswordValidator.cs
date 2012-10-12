using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Service
{
    // validator that checks that username == password
    class PasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName != password)
            {
                throw new SecurityTokenValidationException();
            }
        }
    }
}
