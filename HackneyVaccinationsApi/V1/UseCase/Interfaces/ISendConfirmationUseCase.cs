using HackneyVaccinationsApi.V1.Boundary.Requests;

namespace HackneyVaccinationsApi.V1.UseCase.Interfaces
{
    public interface ISendConfirmationUseCase
    {
        public void Execute(ConfirmationRequest request);
    }
}
