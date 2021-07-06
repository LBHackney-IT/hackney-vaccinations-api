using System.Text.RegularExpressions;
using LbhNotificationsApi.V1.Controllers.Validators.Interfaces;

namespace LbhNotificationsApi.V1.Controllers.Validators
{
    public class SmsRequestValidator : ISmsRequestValidator
    {
        public bool ValidateSms(string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                throw new ValidationException("Mobile number cannot be blank");
            }
            return true;
        }
    }
}
