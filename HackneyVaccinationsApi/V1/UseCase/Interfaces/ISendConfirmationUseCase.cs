using System.Threading.Tasks;
using HackneyVaccinationsApi.V1.Boundary.Requests;

namespace HackneyVaccinationsApi.V1.UseCase.Interfaces
{
    public interface ISendConfirmationUseCase
    {
        public Task Execute(ConfirmationRequest request);
    }
}
