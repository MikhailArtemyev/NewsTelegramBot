// using Telegram.Bot;
// using Telegram.Bot.Types;
//
// namespace AllTagBot.Entity;
//
// public class User
// {
//     public string FirstName { get; init; }
//     public string LastName { get; init; }
//     public long PersonalId { get; init; }
//     public long DmChatId { get; init; }
//     
//     public List<Permissions> Permissions { get; set; }
//
//     private string? _message;
//     private readonly ITelegramBotClient _botClient;
//     
//     public User(ITelegramBotClient botClient ,Message message)
//     {
//         var from = message.From!;
//         FirstName = from.FirstName;
//         LastName = from.LastName ?? string.Empty;
//         PersonalId = from.Id;
//         DmChatId = message.Chat.Id;
//         Permissions = new List<Permissions>();
//         _botClient = botClient;
//     }
//
//     public bool IsMessageSet() => !string.IsNullOrEmpty(_message);
//
//     public void SetMessage(string message) => _message = message;
//
//     public void CancelSending() => _message = null;
//
//     public async Task SendMessage(List<long> subscribers)
//     {
//         if (!Permissions.Contains(Entity.Permissions.Admin)) 
//             await SendMessage(DmChatId, "you don't have permissions to send a message");
//         else
//             foreach (var s in subscribers) await SendMessage(s, _message!);
//         _message = null;
//     }
//
//     public async Task SendMessage(long chatId, string message) => 
//         await PublishJobHost.SendMessage(_botClient, chatId, message);
//     
// }
//
// public enum Permissions
// {
//     Sub, Admin, RegAdmin
// }