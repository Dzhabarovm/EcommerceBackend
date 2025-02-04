using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Infrastructure.Services
{
    public interface ITelegramService
    {
        Task<bool> SendOtpAsync(string phoneNumber);
        Task<bool> VerifyOtpAsync(string phoneNumber, string userInput);
    }

    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDatabase _redis;
        private readonly IConfiguration _config;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(
            IConfiguration config,
            IConnectionMultiplexer redis,
            ILogger<TelegramService> logger)
        {
            _config = config;
            _redis = redis.GetDatabase();
            _logger = logger;
            _botClient = new TelegramBotClient(_config["Telegram:BotToken"]!);
        }

        public async Task<bool> SendOtpAsync(string phoneNumber)
        {
            try
            {
                // 1. Валидация номера
                if (!IsValidPhoneNumber(phoneNumber))
                {
                    _logger.LogWarning($"Invalid phone number: {phoneNumber}");
                    return false;
                }

                // 2. Генерация безопасного OTP
                var secureOtp = GenerateSecureOtp();

                // 3. Получение chat_id из кэша (пример реализации)
                var chatId = await _redis.StringGetAsync($"user:{phoneNumber}:chat_id");
                if (chatId.IsNullOrEmpty)
                {
                    _logger.LogError($"Chat ID not found for {phoneNumber}");
                    return false;
                }

                // 4. Отправка сообщения
                await _botClient.SendMessage(
                    chatId: long.Parse(chatId!),
                    text: $"Ваш код подтверждения: {secureOtp}");

                // 5. Сохранение OTP в Redis (3 попытки, 5 минут)
                var redisKey = $"otp:{phoneNumber}";
                await _redis.StringSetAsync(redisKey, secureOtp, TimeSpan.FromMinutes(5));
                await _redis.StringSetAsync($"{redisKey}:attempts", 3, TimeSpan.FromMinutes(5));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending OTP");
                return false;
            }
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string userInput)
        {
            var redisKey = $"otp:{phoneNumber}";
            var storedOtp = await _redis.StringGetAsync(redisKey);
            var attempts = await _redis.StringGetAsync($"{redisKey}:attempts");

            if (storedOtp.IsNullOrEmpty || attempts.IsNullOrEmpty)
                return false;

            if (int.Parse(attempts!) <= 0)
            {
                await _redis.KeyDeleteAsync(redisKey);
                await _redis.KeyDeleteAsync($"{redisKey}:attempts");
                return false;
            }

            if (storedOtp != userInput)
            {
                await _redis.StringDecrementAsync($"{redisKey}:attempts");
                return false;
            }

            await _redis.KeyDeleteAsync(redisKey);
            await _redis.KeyDeleteAsync($"{redisKey}:attempts");
            return true;
        }

        private static string GenerateSecureOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            return (BitConverter.ToUInt32(bytes) % 1000000).ToString("D6");
        }

        private static bool IsValidPhoneNumber(string phone)
            => Regex.IsMatch(phone, @"^\+992\d{9}$");
    }
}
