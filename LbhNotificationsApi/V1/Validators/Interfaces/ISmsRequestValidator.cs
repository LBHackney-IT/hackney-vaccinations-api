namespace LbhNotificationsApi.V1.Controllers.Validators.Interfaces
{
    public interface ISmsRequestValidator
    {
        bool ValidateSms(string mobileNumber);
    }
}
