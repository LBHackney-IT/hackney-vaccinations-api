using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using Notify.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.Gateways
{
    public class NotifyGateway : INotifyGateway
    {
        private NotificationClient _client;

        public NotifyGateway()
        {
        }
        public bool SendEmailNotification(EmailNotificationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ServiceKey))
            {
                throw new ArgumentNullException(request.ServiceKey);
            }
            if (string.IsNullOrWhiteSpace(request.TemplateId))
            {
                throw new ArgumentNullException(request.TemplateId);
            }
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentNullException(request.Email);
            }
            _client = new NotificationClient(request.ServiceKey);
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
            if (string.IsNullOrWhiteSpace(request.ServiceKey))
            {
                throw new ArgumentNullException(request.ServiceKey);
            }
            if (string.IsNullOrWhiteSpace(request.TemplateId))
            {
                throw new ArgumentNullException(request.TemplateId);
            }
            if (string.IsNullOrWhiteSpace(request.MobileNumber))
            {
                throw new ArgumentNullException(request.MobileNumber);
            }
            _client = new NotificationClient(request.ServiceKey);
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
                _client.SendSms(request.MobileNumber, request.TemplateId, personalisation);

            }
            else
            {
                _client.SendSms(request.MobileNumber, request.TemplateId);
            }
            return true;
        }


        public async Task<IEnumerable<NotifyTemplate>> GetTaskAllTemplateAsync(string serviceKey)
        {
            try
            {
                _client = new NotificationClient(serviceKey);
                var response = await _client.GetAllTemplatesAsync().ConfigureAwait(false);
                var results = response?.templates.Select(x => new NotifyTemplate
                {
                    Body = x.body,
                    CreatedDate = x.created_at,
                    Id = x.id,
                    Name = x.name,
                    Subject = x.subject,
                    Type = x.type,
                    Version = x.version,
                    UpdatedDate = x.updated_at,
                    LetterContactBlock = x.letter_contact_block,
                    CreatedBy = x.created_by
                });
                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
