using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Infrastructure.Persistence.Validators
{
    public class UserProfileValidator : AbstractValidator<DTOUserProfile>
    {
        public UserProfileValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage("Kullanıcı ismi zorunlu!");
            RuleFor(x => x.UserId).NotNull().WithMessage("Kullanıcı Id zorunlu!");
            RuleFor(x => x.File)
                       .Must(BeAValidFileType)
                       .WithMessage("Dosya sadece PNG veya JPG olabilir!");
        }

        private bool BeAValidFileType(IFormFile file)
        {
            if (file == null)
            {
                return true; 
            }

            var allowedExtensions = new[] { ".png", ".jpg" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }
    }
}
