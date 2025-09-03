using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public class SubscriberUser(ITelegramBotClient botClient, Message message, string g) 
    : StaticUser(botClient, message,g)
{
    public override PermissionStatus PermissionStatus { get; set; } = PermissionStatus.Subscriber;
    public override async Task<PermissionStatus> SetPermissionStatus(Message message)
    {
        var formattedMessage = message.Text!.ToLower();
        if (formattedMessage.Contains(UnsubWord))
        {
            await SendMessage(DmChatId, "You unsubscribed");
            return PermissionStatus.Static;
        }
        return 
            await AdminRegistrationDialog(formattedMessage) 
            ?? await GodRegistrationDialog(formattedMessage)
            ?? PermissionStatus;
    }

    public override async Task ReceiveAsyncMessage(string message) 
        => await SendMessage(DmChatId, message);
}