using FluentValidation;
using GrpcCountryApi.Protos.v1;

namespace GrpcCountryApi.Web.Validator
{
    public class CountryCreateRequestValidator : AbstractValidator<CountryCreateRequest>
    {
        public CountryCreateRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("Name is mandatory.");
            RuleFor(request => request.Description).MinimumLength(5).WithMessage("Description is mandatory and be longer than 5 characters");
        }
    }
}
