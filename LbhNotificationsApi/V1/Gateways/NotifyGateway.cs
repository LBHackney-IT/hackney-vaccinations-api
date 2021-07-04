using System.Collections.Generic;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using Notify.Client;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.Infrastructure;

namespace LbhNotificationsApi.V1.Gateways
{
    public class NotifyGateway : INotifyGateway
    {
        private readonly NotificationClient _client;

        public NotifyGateway()
        {
        }
        public bool SendEmailNotification(EmailNotificationRequest request)
        {
            var personalisation = new Dictionary<string, dynamic>();
            if (request.PersonalisationParams != null)
            {
                foreach (var requestPersonalisationParam in request.PersonalisationParams)
                {
                    personalisation.Add(requestPersonalisationParam.Key, requestPersonalisationParam.Value);
                }
            }

            if (personalisation.Count > 0)
            {
                _client.SendEmail(request.Email, request.TemplateId, personalisation);
            }
            else
            {
                _client.SendEmail(request.Email, request.TemplateId);
            }
            return true;
        }

        public bool SendTextMessageNotification(SmsNotificationRequest request)
        {
            var personalisation = new Dictionary<string, dynamic>();
            if (request.PersonalisationParams != null)
            {
                foreach (var requestPersonalisationParam in request.PersonalisationParams)
                {
                    personalisation.Add(requestPersonalisationParam.Key, requestPersonalisationParam.Value);
                }
            }

            if (personalisation.Count > 0)
            {
                _client.SendEmail(request.MobileNumber, request.TemplateId, personalisation);
            }
            else
            {
                _client.SendEmail(request.MobileNumber, request.TemplateId);
            }
            return true;
        }
    }
}
