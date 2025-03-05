using Aurora.Domain.Core.Telegram.Commands;

namespace Aurora.EndPoints.SerpentsBot.Test.CommandParseTest;

public class CommandHandlerTest
{
    [Theory]
    [ClassData(typeof(CommandTextGenerator))]
    public void NewCommandShouldBeWithCorrectType(CommandTextElement commandElement)
        => Assert.Equal(new TelegramCommand(commandElement.CommandText).TelegramCommandType, commandElement.TelegramCommandType);
}