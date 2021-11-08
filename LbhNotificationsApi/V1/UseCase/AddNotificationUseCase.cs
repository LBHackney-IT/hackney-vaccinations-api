using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Common.Enums;

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

        public async Task<Guid> ExecuteAsync(NotificationObjectRequest request)
        {
            var messageSent = new List<string>();
            if (request.NotificationType.Equals(NotificationType.Email) || request.RequireEmailNotification)
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
            if (request.NotificationType.Equals(NotificationType.Text) || request.RequireSmsNotification)
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
                TargetType = request.TargetType.ToString(),
                Message = request.Message,
                Email = request.Email,
                RequireEmailNotification = request.RequireEmailNotification,
                TemplateId = request.TemplateId,
                ServiceKey = request.ServiceKey,
                RequireLetter = request.RequireLetter,
                NotificationType = request.NotificationType.ToString(),
                MobileNumber = request.MobileNumber,
                RequireSmsNotification = request.RequireSmsNotification,
                PersonalisationParams = request.PersonalisationParams,
                RequireAction = request.RequireAction,
                User = request.User,
                IsMessageSent = messageSent
            };
            await _gateway.AddAsync(notification).ConfigureAwait(false);
            return notification.Id;
        }
    }
}
