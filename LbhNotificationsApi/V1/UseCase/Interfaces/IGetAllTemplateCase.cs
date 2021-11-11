using LbhNotificationsApi.V1.Boundary.Response;
using Notify.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.UseCase.Interfaces
{
    public interface IGetAllTemplateCase
    {
        Task<IEnumerable<NotifyTemplate>> ExecuteAsync(string query);
    }
}
