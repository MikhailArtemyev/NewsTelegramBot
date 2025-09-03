using AllTagBot.Entity.Users;
using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity;

public class MessageRouting(ITelegramBotClient botClient, IConfigItem configItem) : IMessageRouting
{
    private readonly Dictionary<long, IUser> _users = new();
    
    public async Task HandleMessageChat(Message message)
    {
        if (_users.TryGetValue(message.From!.Id, out var user)) 
            await user.HandleMessage(message);
    }
    
    public async Task HandleMessageDm(Message message)
    {
        var userId = message.From!.Id;
        _users.TryAdd(userId, new NewUser(botClient, message));
        
        var (newU, changed) = 
            await _users[userId].SetNewUserRightsByCurrent(botClient, message, configItem, _users);
        
        if (changed) _users[userId] = newU;
        else await _users[userId].HandleMessage(message);
    }
}