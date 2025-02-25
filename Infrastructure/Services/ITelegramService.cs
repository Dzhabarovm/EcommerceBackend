using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace Infrastructure.Services
{
    public interface ITelegramService
    {
        Task<bool> SendOtpAsync(string phoneNumber);
        Task<bool> VerifyOtpAsync(string phoneNumber, string userInput);
    }
}
