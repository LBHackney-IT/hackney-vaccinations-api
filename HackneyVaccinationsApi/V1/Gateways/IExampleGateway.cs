using System.Collections.Generic;
using HackneyVaccinationsApi.V1.Domain;

namespace HackneyVaccinationsApi.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
