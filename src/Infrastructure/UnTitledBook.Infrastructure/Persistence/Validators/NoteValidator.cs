using FluentValidation;
using UnTitledBook.Application.DTOs;

namespace UnTitledBook.Infrastructure.Persistence.Validators
{
    public class NoteValidator : AbstractValidator<DTONote>
    {
        public NoteValidator()
        {
             RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Not içerigi boş olamaz!");

            RuleFor(x => x.BookId)
              .NotEmpty()
              .WithMessage("Kitap Id boş olamaz!");

        }
    }
}
