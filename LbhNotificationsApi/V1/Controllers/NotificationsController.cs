using System;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LbhNotificationsApi.V1.Controllers
{
    [ApiController]
    //TODO: Rename to match the APIs endpoint
    [Route("api/v1/notifications")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    //TODO: rename class to match the API name
    public class NotificationsController : BaseController
    {
        private readonly ISendSmsNotificationUseCase _sendSmsNotificationUseCase;
        private readonly ISendEmailNotificationUseCase _sendEmailNotificationUseCase;
        public NotificationsController(ISendSmsNotificationUseCase sendSmsNotificationUseCase,
            ISendEmailNotificationUseCase sendEmailNotificationUseCase)
        {
            _sendSmsNotificationUseCase = sendSmsNotificationUseCase;
            _sendEmailNotificationUseCase = sendEmailNotificationUseCase;
        }


        /// <summary>
        /// ...
        /// </summary>
        /// <response code="201">...</response>
        /// <response code="400">No ? found for the specified ID</response>
        [HttpPost]
        [Route("sms")]
        //TODO: rename to match the identifier that will be used
        public IActionResult CreateSMSNotification(SmsNotificationRequest request)
        {
            _sendSmsNotificationUseCase.Execute(request);
            return Created(new Uri("http://test"), null);
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <response code="201">...</response>
        /// <response code="400">No ? found for the specified ID</response>
        [HttpPost]
        [Route("email")]
        //TODO: rename to match the identifier that will be used
        public IActionResult CreateEmailNotification(EmailNotificationRequest request)
        {
            _sendEmailNotificationUseCase.Execute(request);
            return Created(new Uri("http://test"), null);
        }
    }
}
