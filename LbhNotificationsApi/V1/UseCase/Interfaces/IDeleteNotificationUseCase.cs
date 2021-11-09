using LbhNotificationsApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;

/// <summary>
/// Summary description for Class1
/// </summary>
public interface IDeleteNotificationUseCase
{
    public Task<ActionResponse> ExecuteAsync(Guid id);
}
