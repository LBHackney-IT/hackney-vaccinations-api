using FluentValidation;
using LbhNotificationsApi.V1.Boundary.Requests;

namespace LbhNotificationsApi.V1.Validators
{
    public class ApprovalRequestValidator : AbstractValidator<UpdateRequest>
    {
        public ApprovalRequestValidator()
        {
            RuleFor(p => p.ActionNote).NotEmpty();
            RuleFor(p => p.ActionType).NotNull().IsInEnum().WithMessage("{PropertyName} is required.");
        }
    }
}
