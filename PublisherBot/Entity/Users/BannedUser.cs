using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public class BannedUser(ITelegramBotClient botClient , IUser user) : BaseUser(botClient, user)
{
    public override PermissionStatus PermissionStatus { get; set; } = PermissionStatus.Banned;
    
    private const string BannedMessage = 
        "Sorry, you got banned ;(\n" +
        "For more information contact one of our committee members";
    
    public override async Task<PermissionStatus> SetPermissionStatus(Message message)
    {
        await SendMessage(DmChatId, BannedMessage);
        return PermissionStatus;
    }
    
}