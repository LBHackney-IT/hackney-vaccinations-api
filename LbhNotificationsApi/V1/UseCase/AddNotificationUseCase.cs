using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace LbhNotificationsApi.V1.UseCase
{
    public class AddNotificationUseCase : IAddNotificationUseCase
    {
        private readonly INotificationGateway _gateway;
        private readonly INotifyGateway _notifyGateway;

        public AddNotificationUseCase(INotificationGateway gateway, INotifyGateway notifyGateway)
        {
            _gateway = gateway;
            _notifyGateway = notifyGateway;
        }

        public async Task<Guid> ExecuteAsync(NotificationRequestObject request)
        {
            List<string> messageSent = new List<string>();
            if (request.RequireEmailNotification)
            {
                var emailRequest = new EmailNotificationRequest
                {
                    ServiceKey = request.ServiceKey,
                    TemplateId = request.TemplateId,
                    Email = request.Email,
                    PersonalisationParams = request.PersonalisationParams
                };
                if (_notifyGateway.SendEmailNotification(emailRequest))
                    messageSent.Add("Email Sent");

                messageSent.Add("Email fail to sent");
            }
            if (request.RequireSmsNotification)
            {
                var smsRequest = new SmsNotificationRequest()
                {
                    ServiceKey = request.ServiceKey,
                    TemplateId = request.TemplateId,
                    MobileNumber = request.MobileNumber,
                    PersonalisationParams = request.PersonalisationParams
                };
                if (_notifyGateway.SendTextMessageNotification(smsRequest))
                    messageSent.Add("SMS Sent");

                messageSent.Add("SMS fail to sent");
            }
            var notification = new Notification
            {
                TargetId = request.TargetId,
                TargetType = request.TargetType,
                Message = request.Message,
                Email = request.Email,
                RequireEmailNotification = request.RequireEmailNotification,
                TemplateId = request.TemplateId,
                ServiceKey = request.ServiceKey,
                RequireLetter = request.RequireLetter,
                NotificationType = request.NotificationType,
                MobileNumber = request.MobileNumber,
                RequireSmsNotification = request.RequireSmsNotification,
                PersonalisationParams = request.PersonalisationParams,
                RequireAction = request.RequireAction,
                User = request.User,
                IsMessageSent = messageSent.ToArray()
            };
            await _gateway.AddAsync(notification).ConfigureAwait(false);
            return notification.TargetId;
        }

        private static string GetMessage(TargetType targetType)
        {
            var message = string.Empty;
            switch (targetType)
            {
                case TargetType.FailedDirectDebits:
                    message = "Direct Debit failed";
                    break;
                case TargetType.MissedServiceCharge:
                    message = "3 missed service charge payments have exceeded the tolerance period";
                    break;
                case TargetType.Estimates:
                    message = "2021-2022 estimates have been sent for your approval";
                    break;
                case TargetType.Actuals:
                    message = "2019-2020 Actuals have been approved";
                    break;
                case TargetType.ViewNewProperty:
                    message = "A new property has been added  12/12/12";
                    break;
                case TargetType.ValuateNewProperty:
                    message = "A new property has been added  12/12/12";
                    break;
                case TargetType.ViewNewTenancy:
                    message = "2 Adjustments approved  12/12/12";
                    break;
                case TargetType.SuspenseAccount:
                    message = "A Journal Transfer is awaiting your approval  12/12/12";
                    break;
                default:
                    break;
            }
            return message;
        }
    }
}
