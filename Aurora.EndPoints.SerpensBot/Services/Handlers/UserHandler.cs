using System.Text;
using Aurora.Domain.Core.Context;
using Aurora.Domain.Core.Telegram.Commands;
using Aurora.Domain.Core.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Aurora.EndPoints.SerpensBot.Services.Handlers;

public partial class UpdateHandler
{
    private async Task<Message> ShowReputation(Message msg)
    {
        var sender = msg.From;

        TelegramUser? user = null;
        await using (var context = new AppDbContext())
        {
            //GetOrCreate
            user = context.Users.FirstOrDefault(x => x.Id == sender!.Id);
            if (user == null)
            {
                context.Users.Add(new TelegramUser { Id = sender!.Id, Status = 0 });
                await context.SaveChangesAsync();
                user = context.Users.FirstOrDefault(x => x.Id == sender!.Id);
            }
        }

        var sb = new StringBuilder();
        sb.AppendLine($"{sender!.FirstName} - {(user!.Status >= 500 ? "красавчик" : "еблуша")}");
        sb.AppendLine(
            $"\n{(user.Status >= 500 ? $"У него целых" : "У него всего лишь")} {user.Status} ким респонсибле.");

        logger.LogInformation($"Просмотр состояния пользователя: {user.Id}");

        return await bot.SendMessage(msg.Chat, sb.ToString());
    }

    private async Task<Message> ChangeReputationStatus(Message msg, TelegramCommand command)
    {
        //Получить сообщение на которое был ответ
        var to = msg.ReplyToMessage?.From;
        var from = msg.From;

        var result = CheckSenderValid();

        if (!result.HasValue)
        {
            return msg;
        }

        if (!result.Value)
        {
            return await bot.SendMessage(msg.Chat, "Отказано");
        }

        await using (var context = new AppDbContext())
        {
            //GetOrCreate
            var user = context.Users.FirstOrDefault(x => x.Id == to!.Id);
            if (user == null)
            {
                context.Users.Add(new TelegramUser { Id = to!.Id, Status = 0 });
                await context.SaveChangesAsync();
                user = context.Users.FirstOrDefault(x => x.Id == to!.Id);
            }

            //Update
            user!.UpdateStatus(command.ReputationValue, command.ReputationStatus);

            //SaveChanges
            await context.SaveChangesAsync();
        }

        var sb = new StringBuilder();
        sb.AppendLine($"{to!.FirstName} - {(command.ReputationStatus ? "красавчик" : "матершиник")}");
        sb.AppendLine($"\nЗаслуженно получает {msg.Text} ким респонсибле.");

        logger.LogInformation($"Изменение состояния пользователя:" +
                              $" {to.Id}:{to.FirstName} {(command.ReputationStatus ? "+" : "-")}{command.ReputationValue}");

        return await bot.SendMessage(msg.Chat, sb.ToString());

        bool? CheckSenderValid()
        {
            //Сообщение должно быть ответом на прошлое
            if (from == null || to == null)
            {
                return null;
            }

            //Получателем не может быть бот или отправитель
            return bot.BotId != to.Id && to.Id != from.Id;
        }
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