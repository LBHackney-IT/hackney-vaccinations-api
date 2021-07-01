using HackneyVaccinationsApi.V1.Boundary.Response;

namespace HackneyVaccinationsApi.V1.UseCase.Interfaces
{
    public interface IGetAllUseCase
    {
        ResponseObjectList Execute();
    }
}
