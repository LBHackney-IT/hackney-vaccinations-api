using Hellang.Middleware.ProblemDetails;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.Gateways
{
    public class NotifyGateway : INotifyGateway
    {
        private NotificationClient _client;
        private readonly ILogger<NotifyGateway> _logger;

        public NotifyGateway(ILogger<NotifyGateway> logger)
        {
            _logger = logger;
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

        public async Task<IEnumerable<NotifyTemplate>> GetAllTemplateAsync(string serviceKey)
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
            catch (Exception ex)
            {
                _logger.LogError($"Calling Notify.GetAllTemplatesAsync {Environment.NewLine} {ex.Message}{Environment.NewLine} {ex.InnerException?.Message}");
                throw;
            }
        }


        public async Task<NotifyTemplate> GetTemplateByAsync(string id, string serviceKey)
        {
            try
            {
                await GetNotificationByIdAsync(id, serviceKey).ConfigureAwait(false);
                _client = new NotificationClient(serviceKey);
                var response = await _client.GetTemplateByIdAsync(id).ConfigureAwait(false);
                var results = new NotifyTemplate
                {
                    Body = response.body,
                    CreatedDate = response.created_at,
                    Id = response.id,
                    Name = response.name,
                    Subject = response.subject,
                    Type = response.type,
                    Version = response.version,
                    UpdatedDate = response.updated_at,
                    LetterContactBlock = response.letter_contact_block,
                    CreatedBy = response.created_by
                };
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Calling Notify.GetTemplateByAsync {Environment.NewLine} {ex.Message}{Environment.NewLine} {ex.InnerException?.Message}");
                throw;
            }
        }
        public async Task<GovNotificationResponse> GetNotificationByIdAsync(string id, string serviceKey)
        {
            try
            {
                _client = new NotificationClient(serviceKey);
                var response = await _client.GetNotificationByIdAsync(id).ConfigureAwait(false);

                return new GovNotificationResponse
                {
                    Id = response.id,
                    CompletedAt = response.completedAt,
                    EmailAddress = response.emailAddress,
                    Body = response.body,
                    CreatedByName = response.createdByName,
                    Line1 = response.line1,
                    Line2 = response.line2,
                    Line3 = response.line3,
                    Line4 = response.line4,
                    Line5 = response.line5,
                    Line6 = response.line6,
                    SentAt = response.sentAt,
                    Status = response.status,
                    Subject = response.subject,
                    PhoneNumber = response.phoneNumber,
                    Postage = response.postage,
                    Reference = response.reference,
                    Type = response.type,
                    Template =
                    new Template
                    {
                        Id = response.template?.id,
                        Path = response.template?.uri,
                        Version = response.template.version
                    },

                };
            }
            catch (NotifyClientException error)
            {
                throw new ProblemDetailsException(404, error);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Calling Notify.GetTemplateByAsync {Environment.NewLine} {ex.Message}{Environment.NewLine} {ex.InnerException?.Message}");
                throw;
            }
        }


        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }
}
