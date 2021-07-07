using System.Collections.Generic;
using LbhNotificationsApi.V1.Domain;

namespace LbhNotificationsApi.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
