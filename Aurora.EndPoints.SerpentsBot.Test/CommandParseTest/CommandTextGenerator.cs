using Aurora.Domain.Core.Telegram.Commands;
using Aurora.EndPoints.SerpensBot.Services.Handlers;
using Aurora.EndPoints.SerpentsBot.Test.Base;

namespace Aurora.EndPoints.SerpentsBot.Test.CommandParseTest;

public record CommandTextElement(string CommandText, TelegramCommandType TelegramCommandType);

public class CommandTextGenerator
    : DataGenerator<CommandTextElement>
{
    protected override IEnumerable<CommandTextElement> GetData()
    {
        yield return new CommandTextElement("/alert   ", TelegramCommandType.Alert);
        yield return new CommandTextElement(" /alert", TelegramCommandType.Other);
        yield return new CommandTextElement("/alert@penis", TelegramCommandType.Other);
        yield return new CommandTextElement("/alert@", TelegramCommandType.Other);
        yield return new CommandTextElement("/alert@hydra_bank_alert_bot", TelegramCommandType.Alert);
    }
}