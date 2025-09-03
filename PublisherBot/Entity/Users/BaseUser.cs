using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public abstract class BaseUser : IUser
{
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
    public long PersonalId { get; init; }
    public long DmChatId { get; init; }

    public abstract PermissionStatus PermissionStatus { get; set; }

    private readonly ITelegramBotClient _botClient;

    
    protected const string SubWord = "subscribe";
    protected const string AdminWord = "admin me";
    protected const string Confirm = "confirm";
    protected const string Cancel = "cancel";
    
    protected const string PermissionDeniedSend = 
        "You do not have permissions to send publish messages.\n" +
        "To be able to publish messages, please register as an admin.";
    
    protected BaseUser(ITelegramBotClient botClient, Message message)
    {
        var from = message.From!;
        PersonalId = from.Id;
        FirstName = from.FirstName;
        LastName = from.LastName ?? string.Empty;
        DmChatId = message.Chat.Id;
        _botClient = botClient;
    }
    
    protected BaseUser(ITelegramBotClient botClient , IUser user)
    {
        PersonalId = user.PersonalId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        DmChatId = user.DmChatId;
        _botClient = botClient;
    }

    public abstract Task<PermissionStatus> SetPermissionStatus(Message message);

    public IUser Inject(dynamic injection) => this;
    
    public virtual async Task HandleMessage(Message message) { }
    
    public virtual async Task ReceiveAsyncMessage(string message){}
    
    private static async Task SendMessage( ITelegramBotClient bc,long chatId, string message) =>
        await bc.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            cancellationToken: new CancellationToken());

    protected async Task SendMessage(long chatId, string message) =>
        await SendMessage(_botClient, chatId, message);
}