using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Infrastructure.Persistence.Validators
{
    public class BookValidator : AbstractValidator<DTOBook>
    {
        public BookValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Kitap ismi boş olamaz!")
            .MaximumLength(50)
            .WithMessage("Kitap ismi boş 50 karakterden olmamalıdır!");

            RuleFor(x => x.Description)
           .NotEmpty()
            .WithMessage("Açıklama ismi boş olamaz!");


            RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Kullanıcı Id boş olamaz!");

            RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage("Yazar ismi boş olamaz!");

            RuleFor(x => x.ISBN)
            .NotEmpty()
            .WithMessage("ISBN numarası boş olamaz!");

            RuleFor(x => x.Room)
            .NotEmpty()
            .WithMessage("Kitap oda numarası boş olamaz!");

            RuleFor(x => x.Section)
            .NotEmpty()
            .WithMessage("Kitap bölümü  boş olamaz!");

            RuleFor(x => x.ShelfNumber)
            .NotEmpty()
            .WithMessage("Kitap raf numarası boş olamaz!");

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
