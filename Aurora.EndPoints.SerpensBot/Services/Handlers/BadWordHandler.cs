using Telegram.Bot;
using Telegram.Bot.Types;

namespace Aurora.EndPoints.SerpensBot.Services.Handlers;

public partial class UpdateHandler
{
    private Task<Message> HandleBadWords(Message msg)
        => bot.SendMessage(msg.Chat.Id, "Не матерись, долбаеб", replyParameters: new ReplyParameters()
        {
            ChatId = msg.Chat.Id,
            MessageId = msg.MessageId,
        });
}