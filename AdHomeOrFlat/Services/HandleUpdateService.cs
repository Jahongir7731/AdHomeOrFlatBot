using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AdHomeOrFlat.Services
{
    public class HandleUpdateService
    {
        private readonly ILogger<HandleUpdateService> _logger;
        private readonly ITelegramBotClient _botClient;
        public HandleUpdateService(ILogger<HandleUpdateService> logger, ITelegramBotClient botClient) 
        { 
            _logger = logger;
            _botClient = botClient;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageRecieved(update.Message),
                UpdateType.CallbackQuery => BotOnCallBackQueryRecieved(update.CallbackQuery),
                
                _ => UnknownUpdateTypeHandler(update) //default value
            };
            try 
            {
                await handler;
            }
            catch (Exception ex) { await HandlerErrorAsync(ex); }
        }

        private async Task BotOnCallBackQueryRecieved(CallbackQuery callbackQuery)
        {
            await _botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"({callbackQuery.Data})"
                );
        }

        private async Task BotOnMessageRecieved(Message message)
        {
            _logger.LogInformation($"Message keldi: {message.Type}");

            if (message.Text == "/start")
            {
                await _botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Assalomu aleykum, qaleysiz?");
            }

            try
            {

                if (!string.IsNullOrEmpty(message.Text))
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: message!.Chat.Id,
                        text: "Uyni Rasmini joylang!"
                        //replyMarkup: replyMarkup
                    );
                }
                else if (message.Photo != null && message.Photo.Length > 0)
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: message!.Chat.Id,
                        text: "Bu uy haqida ma'lumot bering (qisqa ma'lumot)!"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("" + ex);
            }

        }

        private Task UnknownUpdateTypeHandler(Update update)// qiymat qaytarmaydi
        {
            _logger.LogInformation($"Unkown update type : {update.Type}");
            return Task.CompletedTask;
        }
        private Task HandlerErrorAsync(Exception ex)
        {
            var ErrorMessage = ex switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error :\n{apiRequestException.ErrorCode}",
                _ => ex.ToString()
            };

            _logger.LogInformation(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
