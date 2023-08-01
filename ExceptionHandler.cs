using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace TgDentomoBot
{
    public class ExceptionHandler : Exception
    {
        /// <summary>
        /// Every error handler
        /// </summary>
        /// <param name="botClient">Telegram client</param>
        /// <param name="exception">Error type</param>
        /// <param name="cancellationToken">Cnc token</param>
        /// <returns></returns>
        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}\n{botClient}\n{cancellationToken}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
