using System.Linq.Expressions;
using Telegram.Bot.Types;

namespace AllTagBot.Interfaces;

public interface IUser
{
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
    
    public long PersonalId { get; init; }
    
    public long DmChatId { get; init; }

    public PermissionStatus PermissionStatus { get; set; }

    public Task<PermissionStatus> SetPermissionStatus(Message message);

    public IUser Inject(dynamic injection);
    
    public Task HandleMessage(Message message);
    
    public Task ReceiveAsyncMessage(string message);
}

public enum PermissionStatus
{
    New, Static, Subscriber, AdminRegistration, Admin, God, Banned
}