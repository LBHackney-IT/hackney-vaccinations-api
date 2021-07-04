using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        Task<bool> SendEmailConfirmation(string email, string slot);
        Task<bool> SendTextMessageConfirmation(string mobileNumber, string slot);
    }
}
