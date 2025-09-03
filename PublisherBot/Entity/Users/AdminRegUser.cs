using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public class AdminRegUser(ITelegramBotClient botClient, Message message, IConfigItem config)
    : StaticUser(botClient, message, config.SuperAccess)
{
    public override PermissionStatus PermissionStatus { get; set; } = PermissionStatus.AdminRegistration;
    private readonly string _publisherPassword = config.ChatPassword;
    
    private const string PasswordIncorrectMessage = "Password is incorrect";
    private const string PublisherRegisteredConfirmation = 
        $"Now you can post messages. To stop beeing an admin type \"{UnAdminWord}\"";

    public override async Task<PermissionStatus> SetPermissionStatus(Message message)
    {
        if (message.Text == _publisherPassword)
        {
            await SendMessage(DmChatId, PublisherRegisteredConfirmation);
            return PermissionStatus.Admin;
        }
        if (message.Text!.ToLower() == Cancel)
        {
            await SendMessage(DmChatId, "canceled");
            return PermissionStatus.Static;
        }
        await SendMessage(DmChatId, PasswordIncorrectMessage);
        return PermissionStatus;
    }
}