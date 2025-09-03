using AllTagBot.Entity.Users;
using AllTagBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllTagBot.Entity;

public static class StaticHelper
{
    public static async Task<(IUser, bool)> SetNewUserRightsByCurrent(
        this IUser user, 
        ITelegramBotClient bc,
        Message message, 
        IConfigItem configItem, 
        Dictionary<long, IUser> users
        )
    {
        var newPermissions = await user.SetPermissionStatus(message);
        if (newPermissions == user.PermissionStatus) return (user,false);
        IUser res = newPermissions switch
        {
            PermissionStatus.New => new NewUser(bc, message),
            PermissionStatus.Static => new StaticUser(bc, message, configItem.SuperAccess),
            PermissionStatus.Subscriber => new SubscriberUser(bc, message, configItem.SuperAccess),
            PermissionStatus.AdminRegistration => new AdminRegUser(bc, message, configItem),
            PermissionStatus.Admin => new AdminUser(bc, message, configItem, users),
            PermissionStatus.God => new GodUser(bc, message, configItem, users),
            _ => throw new ArgumentOutOfRangeException()
        };
        return (res,true);
    }

    
    
}