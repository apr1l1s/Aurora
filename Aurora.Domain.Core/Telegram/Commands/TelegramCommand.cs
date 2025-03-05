using System.Text.RegularExpressions;

namespace Aurora.Domain.Core.Telegram.Commands;

public class TelegramCommand
{
    public TelegramCommandType TelegramCommandType { get; }

    public string[] PoolVariants { get; init; }
    
    public bool ReputationStatus { get; }
    
    public int ReputationValue { get; }
    
    public bool IsPoll => TelegramCommandType == TelegramCommandType.Poll;
    
    public bool IsDirectMessage { get; }

    public TelegramCommand(string commandText)
    {
        var kimStatusRegex = new Regex(@"^(\+|\-)+(\w*)*$");
        var commandRegex = new Regex(@"^\/(user|alert|poll|git|menu)(@hydra_bank_alert_bot)? *$");
        var pollRegex = new Regex(@"^\/poll(@\w+)?\s+\[([^\]]+)\]\s+({[^}]+}\s*){1,5}$");
        
        var kimStatusMatch = kimStatusRegex.Match(commandText);
        if (kimStatusMatch.Success)
        {
            ReputationStatus = kimStatusMatch.Groups[1].Value == "+";
            ReputationValue = int.TryParse(kimStatusMatch.Groups[2].Value, null, out int kimValue) ? kimValue : 1;
        }

        //Проверить, что это команда
        var commandMatch = commandRegex.Match(commandText);
        if (commandMatch.Success)
        {
            var commandType = commandMatch.Groups[1].Value;
            IsDirectMessage = commandMatch.Groups[2].Value == "@hydra_bank_alert_bot";
            TelegramCommandType = commandType switch
            {
                "alert" => TelegramCommandType.Alert,
                "git" => TelegramCommandType.Git,
                "poll" => TelegramCommandType.Poll,
                "menu" => TelegramCommandType.Menu,
                _ => TelegramCommandType.Other
            };
        }
        else
        {
            TelegramCommandType = TelegramCommandType.Other;
        }

        if (TelegramCommandType == TelegramCommandType.Poll)
        {
            var pollMatch = pollRegex.Match(commandText);
            if (pollMatch.Success)
            {
                //Узнать номер группы комманды
                //Проверить есть ли упоминание бота
                //Начать перечислять все группы и записывать их в список ответов
            }
        }

        if (TelegramCommandType == TelegramCommandType.Other)
        {
            TelegramCommandType = ReputationValue == 0 ? TelegramCommandType.Other : TelegramCommandType.ReputationChange;
        }
    }
}