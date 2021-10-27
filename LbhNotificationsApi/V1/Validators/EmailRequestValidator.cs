using System.Text.RegularExpressions;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Validators.Interfaces;

namespace LbhNotificationsApi.V1.Validators
{
    public class EmailRequestValidator : IEmailRequestValidator
    {
        public bool ValidateEmailRequest(EmailNotificationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ValidationException("Email cannot be blank");
            }
            if (string.IsNullOrWhiteSpace(request.ServiceKey))
            {
                throw new ValidationException("A service key must be provided");
            }
            if (string.IsNullOrWhiteSpace(request.TemplateId))
            {
                throw new ValidationException("A template id must be provided");
            }
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(request.Email);
            if (!match.Success)
            {
                throw new ValidationException("Email invalid");
            }
            return true;
        }
    }
}
