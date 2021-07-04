using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;

namespace LbhNotificationsApi.V1.UseCase.Interfaces
{
    public interface ISendConfirmationUseCase
    {
        public Task Execute(ConfirmationRequest request);
    }
}
