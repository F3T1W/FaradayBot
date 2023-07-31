using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace TgDentomoBot
{
    public class ExceptionHandler : Exception
    {
        /// <summary>
        /// Every error handler
        /// </summary>
        /// <param name="botClient">TgBot instance</param>
        /// <param name="exception">Error type</param>
        /// <param name="cancellationToken">Token to break connection</param>
        /// <returns></returns>
        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
