using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public class StaticUser(ITelegramBotClient botClient, Message message, string godWord) : BaseUser(botClient, message)
{
    public override PermissionStatus PermissionStatus { get; set; } = PermissionStatus.Static;
    
    protected const string UnsubWord = "unsubscribe";
    protected const string UnAdminWord = "unadmin";
    private const string PublisherConversationStart = $"Enter the password. To cancel type \"{Cancel}\"";    
    private const string GodConfirmation = "You have an ultimate access to everything.";
    private const string SubscriptionConfirmationMessage = 
        "Thank you for subscribing. If you'd like to unsubscribe, " + 
        $"type \"{UnsubWord}\" and I will stop sending you the news";
    
    public override async Task<PermissionStatus> SetPermissionStatus(Message message)
    {
        var formattedMessage = message.Text!.ToLower();
        return 
            await AdminRegistrationDialog(formattedMessage) 
            ?? await SubRegistrationDialog(formattedMessage) 
            ?? await GodRegistrationDialog(formattedMessage)
            ?? PermissionStatus;
    }
    
    protected async Task<PermissionStatus?> AdminRegistrationDialog(string formattedMessage)
    {
        if (!formattedMessage.Contains(AdminWord, StringComparison.CurrentCultureIgnoreCase)
            || formattedMessage.Contains(UnAdminWord, StringComparison.CurrentCultureIgnoreCase)) return null;
        await SendMessage(DmChatId, PublisherConversationStart);
        return PermissionStatus.AdminRegistration;
    }
    
    protected async Task<PermissionStatus?> GodRegistrationDialog(string formattedMessage)
    {
        if (!formattedMessage.Contains(godWord, StringComparison.CurrentCultureIgnoreCase)) return null;
        await SendMessage(DmChatId, GodConfirmation);
        return PermissionStatus.God;
    }
    
    private async Task<PermissionStatus?> SubRegistrationDialog(string formattedMessage)
    {
        if (!formattedMessage.Contains(SubWord, StringComparison.CurrentCultureIgnoreCase)
            || formattedMessage.Contains(UnsubWord, StringComparison.CurrentCultureIgnoreCase)) return null;
        await SendMessage(DmChatId, SubscriptionConfirmationMessage);
        return PermissionStatus.Subscriber;
    }
}