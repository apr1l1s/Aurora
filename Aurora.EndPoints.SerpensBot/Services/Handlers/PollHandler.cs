using Telegram.Bot;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.SerpensBot.Services.Handlers;

public partial class UpdateHandler
{
    async Task<Message> SendPoll(Message msg)
    {
        return await bot.SendPoll(msg.Chat, "Курить идем?", PollOptions, isAnonymous: false,
            messageThreadId: msg.MessageThreadId);
    }

    async Task<Message> SendAnonymousPoll(Message msg)
    {
        return await bot.SendPoll(chatId: msg.Chat, "Question", PollOptions, messageThreadId: msg.MessageThreadId);
    }
}