using AllTagBot.Entity;
using AllTagBot.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;


var config = ConfigItem.Get("config.json");

var builder = Host.CreateApplicationBuilder();

builder.Services.AddHostedService<BotHost>();
builder.Services.AddSingleton<IConfigItem>(config);
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(config.BotClientToken));
builder.Services.AddSingleton<IMessageRouting,MessageRouting>();
builder.Services.AddSingleton<IHelpProvider, Helper>();

using var host = builder.Build();
host.Run();