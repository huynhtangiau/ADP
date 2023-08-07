using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Books
{
    public class CreateUpdateBookDtoValidator : AbstractValidator<CreateUpdateBookDto>
    {
        public CreateUpdateBookDtoValidator()
        {
            RuleFor(x => x.Name).Length(3, 200);
            RuleFor(x => x.Price).ExclusiveBetween(0.0f, 999.0f);
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}
