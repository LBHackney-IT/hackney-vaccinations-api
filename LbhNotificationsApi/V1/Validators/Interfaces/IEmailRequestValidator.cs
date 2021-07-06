namespace LbhNotificationsApi.V1.Controllers.Validators.Interfaces
{
    public interface IEmailRequestValidator
    {
        bool ValidateEmail(string email);
    }
}
