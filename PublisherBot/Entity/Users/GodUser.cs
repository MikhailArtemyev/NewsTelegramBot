using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity.Users;

public class GodUser(ITelegramBotClient botClient, Message message, IConfigItem config, Dictionary<long, IUser> users)
    : AdminUser(botClient, message, config, users)
{

    public override PermissionStatus PermissionStatus { get; set; } = PermissionStatus.God;
    
    private Commands? _currentCommand;
    
    public override async Task HandleMessage(Message message)
    {
        await base.HandleMessage(message);
        await HandleRequests(message.Text!);
    }

    private async Task HandleRequests(string message)
    {
        if (message.Contains("//"))
        {
            _currentCommand = null;
            await SendMessage(DmChatId, string.Join('\n', GetTextCommands()));
        }
        else
        {
            if (_currentCommand != null) await OnCommand(message);
            else await SelectCommand(message);
        }
    }

    private async Task SelectCommand(string message)
    {
        var parsed = int.TryParse(message, out var i);
        if (parsed)
        {
            var c = Enum.GetValues<Commands>().Where(c => (int)c == i).ToList();
            if (c.Any())
            {
                var comm = c.First();
                _currentCommand = comm;
                await SendMessage(DmChatId, $"you selected: {comm}. Send a following command");
            }
        }
    }
    
    private async Task OnCommand(string message)
    {
        switch (_currentCommand)
        {
            case Commands.ListOfSubs:
                var list = SubsToList();
                await SendMessage(
                    DmChatId, 
                    list.Count != 0 ? string.Join('\n', list) : "No subs"
                    );
                break;
            case Commands.ListOfAdmins:
                var list2 = AdminsToList();
                await SendMessage(
                    DmChatId, 
                    list2.Count != 0 ? string.Join('\n', list2) : "No admins"
                );
                break;
            case Commands.ListOfStaticUsers:
                var list3 = StaticToList();
                await SendMessage(
                    DmChatId, 
                    list3.Count != 0 ? string.Join('\n', list3) : "No static users"
                );
                break;
            case Commands.ListOfBannedUsers:
                var list4 = BannedToList();
                await SendMessage(
                    DmChatId, 
                    list4.Count != 0 ? string.Join('\n', list4) : "No banned"
                );
                break;
            case Commands.Ban:
                if (long.TryParse(message, out var id)) await BanUser(id);
                else await SendMessage(DmChatId, "Select a valid id");
                break;
            case Commands.Unban:
                if (long.TryParse(message, out var idUnban)) await UnbanUser(idUnban);
                else await SendMessage(DmChatId, "Select a valid id");
                break;
            case Commands.ChangePassword:
                config.ChatPassword = message;
                await SendMessage(DmChatId, $"The password was changed for {message}");
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _currentCommand = null;
    }
    
    
    private async Task BanUser(long userId)
    {
        users[userId] = new BannedUser(botClient, users[userId]);
        await SendMessage(DmChatId, $"User (id: {userId}) has been banned");
    }

    private async Task UnbanUser(long userId)
    {
        var user = users[userId];
        if (user.PermissionStatus == PermissionStatus.Banned)
        {
            users[userId] = new NewUser(botClient, users[userId]);
            await SendMessage(DmChatId, $"User (id: {userId}) has been unbanned");
        }
        else
            await SendMessage(DmChatId, $"No action performed. The user (id: {userId}) was not banned");
    }
    
    public override async Task<PermissionStatus> SetPermissionStatus(Message message)
    {
        var formattedMessage = message.Text!.ToLower();
        if (formattedMessage.Contains(UnAdminWord))
        {
            await SendMessage(DmChatId, "You are no longer an admin");
            return PermissionStatus.Static;
        }
        return PermissionStatus;
    }

    private List<string> ListOf(PermissionStatus status) =>
        users.Where(x => x.Value.PermissionStatus == status)
            .Select(x => $"{x.Value.FirstName} {x.Value.LastName} : {x.Value.PersonalId}").ToList();

    private List<string> SubsToList() => ListOf(PermissionStatus.Subscriber);
    
    private List<string> AdminsToList() => ListOf(PermissionStatus.Admin);
        
    private List<string> StaticToList() => ListOf(PermissionStatus.Static);
    
    private List<string> BannedToList() => ListOf(PermissionStatus.Banned);
    
    private static IEnumerable<string> GetTextCommands()
    {
        var res = new List<string>();
        var count = Enum.GetNames(typeof(Commands)).Length;
        for (var i = 0; i < count; i++)
        {
            res.Add($"{i} : {Enum.GetName(typeof(Commands), i)}");
        }
        
        return res;
    }
    
    private enum Commands
    {
        ListOfSubs, ListOfAdmins, ListOfStaticUsers, ListOfBannedUsers, Ban, Unban, ChangePassword
    }
    
    //Enum.GetName(typeof(MyEnumClass), value)
}