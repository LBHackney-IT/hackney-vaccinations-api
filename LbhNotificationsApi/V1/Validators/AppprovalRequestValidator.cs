using FluentValidation;
using LbhNotificationsApi.V1.Boundary.Requests;

namespace LbhNotificationsApi.V1.Validators
{
    public class ApprovalRequestValidator : AbstractValidator<ApprovalRequest>
    {
        public ApprovalRequestValidator()
        {
            RuleFor(p => p.ApprovalNote).NotEmpty();
            RuleFor(p => p.ApprovalStatus).NotNull().IsInEnum().WithMessage("{PropertyName} is required.");
        }
    }
}
