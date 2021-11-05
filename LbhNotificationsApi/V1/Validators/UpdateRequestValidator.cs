using FluentValidation;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Validators
{
    public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(p => p.ActionNote).NotEmpty().When(s => s.ActionType == ActionType.Approved);
            RuleFor(p => p.ActionType).NotNull().IsInEnum().WithMessage("{PropertyName} is required.");
        }
    }
}
