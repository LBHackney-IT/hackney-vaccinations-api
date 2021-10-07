using System.Text.RegularExpressions;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Validators.Interfaces;

namespace LbhNotificationsApi.V1.Controllers.Validators
{
    public class SmsRequestValidator : ISmsRequestValidator
    {
        public bool ValidateSmsRequest(SmsNotificationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.MobileNumber))
            {
                throw new ValidationException("Mobile number cannot be blank");
            }
            if (string.IsNullOrWhiteSpace(request.ServiceKey))
            {
                throw new ValidationException("A service key must be provided");
            }
            if (string.IsNullOrWhiteSpace(request.TemplateId))
            {
                throw new ValidationException("A template id must be provided");
            }
            return true;
        }
    }
}
