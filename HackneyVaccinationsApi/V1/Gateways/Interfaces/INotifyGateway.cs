using System.Threading.Tasks;

namespace HackneyVaccinationsApi.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        Task<bool> SendEmailConfirmation(string email);
        Task<bool> SendTextMessageConfirmation(string mobileNumber);
    }
}
