using System;

namespace LbhNotificationsApi.V1.Validators
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {

        }
    }
}
