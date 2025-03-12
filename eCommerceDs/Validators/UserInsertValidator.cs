using eCommerceDs.DTOs;
using FluentValidation;

namespace eCommerceDs.Validators
{
    public class UserInsertValidator : AbstractValidator<UserInsertDTO>
    {
        public UserInsertValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
