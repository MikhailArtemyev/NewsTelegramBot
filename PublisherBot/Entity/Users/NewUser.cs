using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public class NewUser : BaseUser
{
    public override PermissionStatus PermissionStatus { get; set; } = PermissionStatus.New;
    
    public NewUser(ITelegramBotClient botClient, Message message) : base(botClient, message) {}
    
    public NewUser(ITelegramBotClient botClient, IUser user) : base(botClient, user) {}
    
    private const string StartMessage = 
        $"Hi. \n\nIf you want to subscribe to us, type \"{SubWord}\" and I will start sending you our news.\n\n"+ 
        $"If you want to sign up as an admin, type \"{AdminWord}\" and consider the fallowing instructions.";
    
    public override async Task<PermissionStatus> SetPermissionStatus(Message message)
    {
        await SendMessage(DmChatId, StartMessage);
        return PermissionStatus.Static;
    }
    
}