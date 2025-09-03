namespace AllTagBot.Interfaces;

public interface IConfigItem
{
    public string BotClientToken { get; init; }
    public string PublishKeyword { get; init; }
    
    public string SuperAccess { get; init; }
    public string ChatPassword { get; set; }
}