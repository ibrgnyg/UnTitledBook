using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Infrastructure.Persistence.Validators
{
    public class UserLoginValidator : AbstractValidator<DTOUserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress().WithMessage("Email zorunlu!");
            RuleFor(x => x.Password).NotNull().WithMessage("Şifre zorunlu!");
        }
    }
}
