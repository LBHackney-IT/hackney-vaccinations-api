using LbhNotificationsApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.UseCase.Interfaces
{
    public interface IGetTemplateByIdUseCase
    {
        Task<NotifyTemplate> ExecuteAsync(string id, string service);
    }
}
