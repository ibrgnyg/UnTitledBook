using FluentValidation;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Infrastructure.Persistence.Validators
{
    public class UserRegisterValidator : AbstractValidator<DTOUserRegister>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.UserName).NotNull().Length(0, 5);
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotNull();
        }
    }
}
