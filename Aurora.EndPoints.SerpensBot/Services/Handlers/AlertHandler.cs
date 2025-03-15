using System.Text;
using Aurora.Domain.Core.Context;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Aurora.EndPoints.SerpensBot.Services.Handlers;

public partial class UpdateHandler
{
    private async Task<Message> SendAlert(Message msg)
    {
        await bot.SendChatAction(msg.Chat, ChatAction.UploadPhoto);
        // using var fileStream = new FileStream("/Files/nehochyn.jpg", FileMode.Open, FileAccess.Read);
        var prompt = GetRandomPrompt();
        var response = await prompter.SendAsync(prompt) ?? "Алиса сдохла";

        await bot.SendMessage(msg.Chat, response, messageThreadId: msg.MessageThreadId);
        return await SendPoll(msg);
    }

    private async Task<Message> SendGit(Message msg)
        => await bot.SendMessage(msg.Chat, "https://github.com/apr1l1s/Aurora", messageThreadId: msg.MessageThreadId);

    private static string GetRandomPrompt()
    {
        var rand = new Random();
        const int min = 1000;
        const int max = 3000;

        var sb = new StringBuilder();

        sb.AppendLine("Можешь написать текст, " +
                      "который будет использован для уведомления о том, что мы с коллегами пойдем курить. ");
        sb.AppendLine($"Текст должен быть длинной где то {rand.Next(min, max)} символов и содержать много смайликов. ");

        switch (rand.Next(min, max))
        {
            case < 1500:
                sb.AppendLine("Так же можешь туда поместить пару слов про то, какая погода на улице. ");
                break;
            case < 2000:
                sb.AppendLine("Так же можешь туда написать случайную шутку. ");
                break;
            case < 2500:
                sb.AppendLine("Так же можешь упомянуть, несколько плюсов перерыва для работы. ");
                break;
            case < 3000:
                sb.AppendLine(
                    "Так же после сообщения добавь загадку и оставь ответ в виде скрытого текста. ");
                break;
        }

        sb.AppendLine("Текст должен быть красив, уникален и смысл в том, что надо покурить, " +
                      "проветриться всем участником беседы.");

        return sb.ToString();
    }
}