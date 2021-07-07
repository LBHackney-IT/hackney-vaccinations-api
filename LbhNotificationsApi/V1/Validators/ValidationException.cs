using System;

namespace LbhNotificationsApi.V1.Controllers.Validators
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {

        }
    }
}
