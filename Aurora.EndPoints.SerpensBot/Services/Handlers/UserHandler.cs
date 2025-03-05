using System.Text;
using Aurora.Domain.Core.Telegram.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Aurora.EndPoints.SerpensBot.Services.Handlers;

public partial class UpdateHandler
{
    private async Task<Message> ChangeReputationStatus(Message msg, TelegramCommand command)
    {
        //Получить сообщение на которое был ответ
        var previousUser = msg?.ReplyToMessage.From;
        var sender = msg.From;

        if (bot.BotId == previousUser.Id)
        {
            return await bot.SendMessage(msg.Chat, "Ебальник офни свой, хуй недорос мне указывать что делать");
        }

        if (previousUser.Id == sender.Id)
        {
            return await bot.SendMessage(msg.Chat,
                "Эй мужичек, а ты уверен, что у тебя достаточно власти себе очки давать?");
        }

        if (previousUser == null)
        {
            return msg;
        }

        //Взять его статус из бд
        //Поменять
        //Записать в базу
        //Выести новое значение
        var sb = new StringBuilder();
        sb.AppendLine($"{previousUser.FirstName} - {(command.ReputationStatus ? "красавчик" : "матершиник")}");
        sb.AppendLine($"\nЗаслуженно получает {msg.Text} ким респонсибле.");
        
        return await bot.SendMessage(msg.Chat, sb.ToString());
    }
    
    private async Task<Message> RequestContactAndLocation(Message msg)
    {
        var replyMarkup = new ReplyKeyboardMarkup(true)
            .AddButton(KeyboardButton.WithRequestLocation("Location"))
            .AddButton(KeyboardButton.WithRequestContact("Contact"));
        return await bot.SendMessage(msg.Chat, "Who or Where are you?", replyMarkup: replyMarkup,
            messageThreadId: msg.MessageThreadId);
    }
}