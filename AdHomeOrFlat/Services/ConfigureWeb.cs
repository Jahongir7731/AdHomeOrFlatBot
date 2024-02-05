using System;
using System.Threading;
using System.Threading.Tasks;
using AdHomeOrFlat.Models;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AdHomeOrFlat.Services
{
    public class ConfigureWeb : IHostedService
    {
        private readonly ILogger<ConfigureWeb> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly BotConfiguration _botConfig;

        public ConfigureWeb(ILogger<ConfigureWeb> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration
            ) 
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            var webhookAddress = $@"{_botConfig.HostAddress}/bot/{_botConfig.Token}";

            _logger.LogInformation("Setting webhook");

            await botClient.SendTextMessageAsync(
             chatId: 686455642,
             text: "Bot ishlab boshladi"
             );

            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken
                );
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            _logger.LogInformation("WebHook removing");

            await botClient.SendTextMessageAsync(
                chatId: 686455642,
                text: "Bot is not working"
                );
        }
    }
}

