using System;
using FluentValidation;
using LbhNotificationsApi.V1.Boundary.Requests;

namespace LbhNotificationsApi.V1.Validators
{
    public class NotificationRequestValidation : AbstractValidator<NotificationRequestObject>
    {
        public NotificationRequestValidation()
        {
            RuleFor(p => p.TargetId).NotNull().NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");
            RuleFor(p => p.TargetType).NotNull().IsInEnum().WithMessage("{PropertyName} is required.");
        }
    }

}
