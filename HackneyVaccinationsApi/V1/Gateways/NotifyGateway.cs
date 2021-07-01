using System.Collections.Generic;
using System.Threading.Tasks;
using Notify.Client;
using HackneyVaccinationsApi.V1.Gateways.Interfaces;
using HackneyVaccinationsApi.V1.Infrastructure;

namespace HackneyVaccinationsApi.V1.Gateways
{
    public class NotifyGateway : INotifyGateway
    {
        private readonly NotificationClient _client;
        private readonly NotifyResources _resources;

        public NotifyGateway(NotifyResources resources)
        {
            _resources = resources;
            _client = new NotificationClient(_resources.NotifyKey);
        }
        public async Task<bool> SendEmailConfirmation(string email)
        {
            var personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("slot", "testing");
            await _client.SendEmailAsync(email, _resources.EmailTemplate, personalisation).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> SendTextMessageConfirmation(string mobileNumber)
        {
            var personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("slot", "testing");
            await _client.SendSmsAsync(mobileNumber, _resources.TextMessageTemplate, personalisation).ConfigureAwait(false);
            return true;
        }
    }
}
