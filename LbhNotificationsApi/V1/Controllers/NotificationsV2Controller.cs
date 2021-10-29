using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using LbhNotificationsApi.V1.Validators;
using LbhNotificationsApi.V1.Validators.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace LbhNotificationsApi.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v2/notifications")]
    [Produces("application/json")]
    
    public class NotificationsV2Controller : BaseController
    {
        private readonly ISendSmsNotificationUseCase _sendSmsNotificationUseCase;
        private readonly ISendEmailNotificationUseCase _sendEmailNotificationUseCase;
        private readonly IEmailRequestValidator _emailRequestValidator;
        private readonly ISmsRequestValidator _smsRequestValidator;
        private readonly IGetAllNotificationCase _getAllNotificationCase;
        private readonly IGetByIdNotificationCase _getByIdNotificationCase;
        private readonly IAddNotificationUseCase _addNotificationUseCase;
        private readonly IUpdateNotificationUseCase _updateNotificationUseCase;
        public NotificationsV2Controller(
            ISendSmsNotificationUseCase sendSmsNotificationUseCase,
            ISendEmailNotificationUseCase sendEmailNotificationUseCase,
            IEmailRequestValidator emailRequestValidator,
            ISmsRequestValidator smsRequestValidator,
            IGetAllNotificationCase getAllNotificationCase,
            IGetByIdNotificationCase getByIdNotificationCase,
            IAddNotificationUseCase addNotificationUseCase,
            IUpdateNotificationUseCase updateNotificationUseCase)
        {
            _sendSmsNotificationUseCase = sendSmsNotificationUseCase;
            _sendEmailNotificationUseCase = sendEmailNotificationUseCase;
            _emailRequestValidator = emailRequestValidator;
            _smsRequestValidator = smsRequestValidator;
            _getAllNotificationCase = getAllNotificationCase;
            _getByIdNotificationCase = getByIdNotificationCase;
            _addNotificationUseCase = addNotificationUseCase;
            _updateNotificationUseCase = updateNotificationUseCase;
        }


        /// <summary>
        /// ...
        /// </summary>
        /// <response code="201">...</response>
        /// <response code="400">No ? found for the specified ID</response>
        [HttpPost]
        [Route("sms")]
        //TODO: rename to match the identifier that will be used
        public IActionResult CreateSmsNotification(SmsNotificationRequest request)
        {
            try
            {
                _smsRequestValidator.ValidateSmsRequest(request);
                _sendSmsNotificationUseCase.Execute(request);
                return Created(new Uri("http://test"), null);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
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
            try
            {
                _emailRequestValidator.ValidateEmailRequest(request);
                _sendEmailNotificationUseCase.Execute(request);
                return Created(new Uri("http://test"), null);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <response code="200">...</response>
        /// <response code="400">Invalid Query Parameter.</response>
        [ProducesResponseType(typeof(NotificationResponseObjectList), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> ListNotificationAsync([FromQuery]NotificationSearchQuery query)
        {
            return Ok(await _getAllNotificationCase.ExecuteAsync(query).ConfigureAwait(false));
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <response code="200">...</response>
        /// <response code="404">No ? found for the specified ID</response>
        /// 
        [ProducesResponseType(typeof(NotificationResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("{targetId:guid}")]
        public async Task<IActionResult> GetAsync(Guid targetId)
        {
            var result = await _getByIdNotificationCase.ExecuteAsync(targetId).ConfigureAwait(false);
            if (result == null)
                return NotFound(targetId);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] NotificationRequestObject request)
        {
        

            var result = await _addNotificationUseCase.ExecuteAsync(request).ConfigureAwait(false);
            return Created(new Uri("http://test"), result);

        }

        [ProducesResponseType(typeof(ActionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        [Route("{targetId}")]
        public async Task<IActionResult> UpdateAsync(Guid targetId, [FromBody] ApprovalRequest request)
        {
            var result = await _getByIdNotificationCase.ExecuteAsync(targetId).ConfigureAwait(false);
            if (result == null)
                return NotFound(targetId);
            var updateResult = await _updateNotificationUseCase.ExecuteAsync(targetId, request).ConfigureAwait(false);
            if (updateResult.Status)
                return Ok(updateResult);

            return BadRequest(updateResult);

        }
    }
}
