using System;
using FluentValidation;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Validators
{
    public class NotificationRequestValidation : AbstractValidator<NotificationRequestObject>
    {
        public NotificationRequestValidation()
        {
            RuleFor(p => p.NotificationType).NotEmpty().WithMessage("{PropertyName} is required.").Must(r=> r != NotificationType.All).WithMessage("Select other {PropertyName}.");
            When(p => p.NotificationType == NotificationType.Screen, () => {
                RuleFor(p => p.TargetId).NotEmpty().WithMessage("{PropertyName} is required.");
                RuleFor(p => p.TargetType).NotNull().IsInEnum().Must(n=> n != TargetType.None).WithMessage("{PropertyName} is required.");
                RuleFor(r => r.Message).NotEmpty().WithMessage("{PropertyName} is required.");
            });
            When(p => p.NotificationType == NotificationType.Email || p.RequireEmailNotification, () => {
                RuleFor(r => r.Email).EmailAddress();
                RuleFor(r => r.TemplateId).NotEmpty().WithMessage("{PropertyName} is required.");
                RuleFor(r => r.ServiceKey).NotEmpty().WithMessage("{PropertyName} is required.");
            });

            When(p => p.NotificationType == NotificationType.Text || p.RequireSmsNotification, () => {
                RuleFor(r => r.MobileNumber).NotEmpty().WithMessage("{PropertyName} is required.");
                RuleFor(r => r.TemplateId).NotEmpty().WithMessage("{PropertyName} is required.");
                RuleFor(r => r.ServiceKey).NotEmpty().WithMessage("{PropertyName} is required.");
            });
        }
    }

}
