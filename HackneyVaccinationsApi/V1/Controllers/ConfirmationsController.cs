using System;
using HackneyVaccinationsApi.V1.Boundary.Requests;
using HackneyVaccinationsApi.V1.Boundary.Response;
using HackneyVaccinationsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackneyVaccinationsApi.V1.Controllers
{
    [ApiController]
    //TODO: Rename to match the APIs endpoint
    [Route("api/v1/confirmations")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    //TODO: rename class to match the API name
    public class ConfirmationsController : BaseController
    {
        private readonly ISendConfirmationUseCase _sendConfirmationUseCase;
        public ConfirmationsController(ISendConfirmationUseCase sendConfirmationUseCase)
        {
            _sendConfirmationUseCase = sendConfirmationUseCase;
        }


        /// <summary>
        /// ...
        /// </summary>
        /// <response code="201">...</response>
        /// <response code="400">No ? found for the specified ID</response>
        [HttpPost]
        //TODO: rename to match the identifier that will be used
        public IActionResult CreateConfirmations(ConfirmationRequest request)
        {
            _sendConfirmationUseCase.Execute(request);
            return Created(new Uri("http://test"), null);
        }
    }
}
