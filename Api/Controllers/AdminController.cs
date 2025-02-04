using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/applications")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IMediator mediator, ILogger<AdminController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications([FromQuery] ApplicationStatus? status)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationsQuery(status));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка заявок");
                return StatusCode(500, new { success = false, error = "Internal server error" });
            }
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveApplication(Guid id, [FromBody] ApproveApplicationRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { success = false, error = "Invalid request body" });
            }

            try
            {
                await _mediator.Send(new ApproveApplicationCommand(id, request.ShopName, request.ShopDescription));
                _logger.LogInformation("Заявка {ApplicationId} успешно одобрена", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при одобрении заявки {ApplicationId}", id);
                return StatusCode(500, new { success = false, error = "Internal server error" });
            }
        }
    }
}
