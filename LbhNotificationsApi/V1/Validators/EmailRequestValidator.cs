using System.Text.RegularExpressions;
using LbhNotificationsApi.V1.Controllers.Validators.Interfaces;

namespace LbhNotificationsApi.V1.Controllers.Validators
{
    public class EmailRequestValidator : IEmailRequestValidator
    {
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ValidationException("Email cannot be blank");
            }
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                throw new ValidationException("Email invalid");
            }
            return true;
        }
    }
}
