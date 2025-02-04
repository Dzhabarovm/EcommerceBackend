using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации с использованием OTP через Telegram.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Конструктор с внедрением зависимостей.
        /// </summary>
        /// <param name="telegramService">Сервис для работы с Telegram OTP.</param>
        /// <param name="logger">Логгер для ведения журнала действий контроллера.</param>
        public AuthController(ITelegramService telegramService, ILogger<AuthController> logger)
        {
            _telegramService = telegramService;
            _logger = logger;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] string phoneNumber)
        {
            var result = await _telegramService.SendOtpAsync(phoneNumber);
            return result ? Ok() : BadRequest("Invalid phone number or service error");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(
            [FromBody] VerifyOtpRequest request)
        {
            var isValid = await _telegramService.VerifyOtpAsync(
                request.PhoneNumber,
                request.Otp);

            if (isValid)
            {
                // Генерация JWT токена или создание сессии
                return Ok(new { Token = "generated_jwt_token" });
            }

            return Unauthorized("Invalid OTP or no attempts left");
        }
    }

    public record VerifyOtpRequest(string PhoneNumber, string Otp);
}
