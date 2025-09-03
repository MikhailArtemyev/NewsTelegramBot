using System;
using AllTagBot.Interfaces;

namespace AllTagBot.Entity;

public class Helper : IHelpProvider
{
    public void PrintMessage(string state, string messageText, string chatId)
    {
        Console.WriteLine(
            $"Chat: {chatId} \nBotState: {state} \nMessage:\n# {messageText} #\n");
    }
    
    public void PrintMessage(string messageText, string chatId)
    {
        Console.WriteLine(
            $"Chat: {chatId} \nMessage:\n'{messageText}'\n");
    }

    public void PrintMessage(string text) => Console.WriteLine(text);
}