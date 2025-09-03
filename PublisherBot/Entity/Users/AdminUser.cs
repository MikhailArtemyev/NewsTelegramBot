using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public class AdminUser(
    ITelegramBotClient botClient, Message message, IConfigItem config, Dictionary<long, IUser> users
    ) : SubscriberUser(botClient, message, config.SuperAccess)
{
    public override PermissionStatus PermissionStatus { get; set; } = PermissionStatus.Admin;

    private string? _hashedMessage;
    private readonly string _triggerWord = config.PublishKeyword;
        
    private const string MessageSavedConfirmation = 
        $"Confirm sending this message by typing \"{Confirm}\" or cancel it by typing \"{Cancel}\".";
    private const string SendCanceledConfirmation = "Canceled sending";
    private const string MessageSentConfirmation = "The message has been posted";
    private const string MessageInQueue= "You already have a message in a sending queue";

    public override async Task HandleMessage(Message message)
    {
        if (MessageIsSet()) await OnMessageSet(message);
        else await OnMessageNotSet(message);
    }
    

    public override async Task<PermissionStatus> SetPermissionStatus(Message message)
    {
        var formattedMessage = message.Text!.ToLower();
        if (formattedMessage.Contains(UnAdminWord))
        {
            await SendMessage(DmChatId, "You are no longer an admin");
            return PermissionStatus.Static;
        }
        return 
            await GodRegistrationDialog(formattedMessage)
            ?? PermissionStatus;
    }

    private async Task OnMessageSet(Message message)
    {
        var t = message.Text!;
        if(t.Contains(_triggerWord)) await SendMessage(DmChatId, MessageInQueue);
        else 
            switch (t)
            {
                case Confirm:
                    await SendMessage(DmChatId, MessageSentConfirmation);
                    _hashedMessage += $"\nby {FirstName} {LastName} (id: {PersonalId})";
                    users.ToList().ForEach(OneUserReceive);
                    _hashedMessage = null;
                    break;
                case Cancel:
                    await SendMessage(DmChatId, SendCanceledConfirmation);
                    _hashedMessage = null;
                    break;
            }
    }

    private async void OneUserReceive(KeyValuePair<long, IUser> u) => 
        await u.Value.ReceiveAsyncMessage(_hashedMessage!);
    

    private async Task OnMessageNotSet(Message message)
    {
        var t = message.Text!;
        if (t.Contains(_triggerWord))
        {
            _hashedMessage = t;
            await SendMessage(DmChatId, MessageSavedConfirmation);
        }
    }
    
    private bool MessageIsSet() => !string.IsNullOrEmpty(_hashedMessage);

}