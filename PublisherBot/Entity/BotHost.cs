using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AllTagBot.Entity;

public class BotHost(IHelpProvider helper, IMessageRouting messageRouting, ITelegramBotClient botClient)
    : IBotHost
{
    private readonly CancellationTokenSource _cts = new ();
    
    private Task HandlePollingErrorAsync(
        ITelegramBotClient bc, Exception exception, 
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:" +
                   $"\n[{apiRequestException.ErrorCode}]" +
                   $"\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        helper.PrintMessage(errorMessage);
        return Task.CompletedTask;
    }
    
    private static bool MandatoryCheck(Update update) =>
        update.Message is { Text : not null };
    
    private static bool IsInChat(Update update) => 
        update.Message!.Chat.Title != null;

    private async Task HandleUpdateAsync(
        ITelegramBotClient bc, Update update, CancellationToken ct)
    {
        if (!MandatoryCheck(update))
        {
            helper.PrintMessage($"bad update: {update.Type}\n");
            return;
        }
        var message = update.Message!;
        
        if (IsInChat(update)) 
            await messageRouting.HandleMessageChat(message);
        else 
            await messageRouting.HandleMessageDm(message);
    }
    
    public async Task StartAsync(CancellationToken ct)
    {
        ReceiverOptions receiverOptions = new() { AllowedUpdates = Array.Empty<UpdateType>() };
        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: _cts.Token
        );
        var me = await botClient.GetMeAsync(cancellationToken: ct);
        helper.PrintMessage($"Start listening for @{me.Username}");
    }
    
    public Task StopAsync(CancellationToken ct) =>
        new (_cts.Cancel);
}