using System.Threading.Tasks;

namespace HackneyVaccinationsApi.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        Task<bool> SendEmailConfirmation(string email, string slot);
        Task<bool> SendTextMessageConfirmation(string mobileNumber, string slot);
    }
}
