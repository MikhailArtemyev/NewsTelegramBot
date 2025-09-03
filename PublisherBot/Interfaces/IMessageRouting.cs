using Telegram.Bot.Types;

namespace AllTagBot.Interfaces;

public interface IMessageRouting
{
    public Task HandleMessageChat(Message message);
    public Task HandleMessageDm(Message message); 
}