using AllTagBot.Interfaces;
using Newtonsoft.Json;
using File = System.IO.File;

namespace AllTagBot.Entity;

public class ConfigItem : IConfigItem
{
    public string BotClientToken { get; init; } = string.Empty;
    
    public string SuperAccess { get; init; } = string.Empty;
    public string PublishKeyword { get; init; }  = string.Empty;
    public string ChatPassword { get; set; } = string.Empty;

    public static ConfigItem Get(string path) => 
        JsonConvert.DeserializeObject<ConfigItem>(File.ReadAllText(path))!;
    
}