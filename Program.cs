using FaradayBot;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgDentomoBot;

string databasePath = "C:\\Users\\folderF3T1W\\AppData\\Roaming\\faraday\\db.sqlite";
string tableName = "ChatItem";
string fieldName = "output";
string orderByField = "createdAt";

using CancellationTokenSource cts = new();

FileSystemWatcher watcher = new()
{
    // Set the path to the directory containing the .sqlite file
    Path = Path.GetDirectoryName(databasePath) ?? "",

    // Set the filter to watch for changes only in the .sqlite file
    Filter = Path.GetFileName(databasePath)
};

// Subscribe to the Changed event, which will be raised whenever the .sqlite file is updated
watcher.Changed += OnFileChanged;

// Start watching for changes
watcher.EnableRaisingEvents = true;
Console.WriteLine("Sqlite is checking.");

// Start listening for Telegram Bot messages
TelegramBotClient botClient = new("6325714020:AAHab93QTiW6-UDRk8bJPKWmZI3x67A98yw");
var receiver = new QueuedUpdateReceiver(botClient);

ReceiverOptions receiverOptions = new() // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: ExceptionHandler.HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

// Keep the program running until cancellation is requested
Console.WriteLine("Bot is running. Press any key to stop.");
Console.ReadKey();

// Cancel receiving messages
cts.Cancel();

// Stop watching for changes when the program is stopped
watcher.EnableRaisingEvents = false;

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var userName = message.From?.FirstName;
    var userId = message.From?.Id;

    string? value = DatabaseExtractor.GetLatestFieldValue(databasePath, tableName, fieldName, orderByField);
    if (value != null)
    {
        string extractedContent = ExtractContentAfterColon(value);

        await botClient.SendTextMessageAsync(chatId, extractedContent, cancellationToken: cancellationToken);
        Console.WriteLine($"Value in {databasePath}.{tableName}.{fieldName}: {extractedContent}");
    }
    else
    {
        await botClient.SendTextMessageAsync(chatId, "Ошибочка!", cancellationToken: cancellationToken);
        Console.WriteLine($"Value not found in {databasePath}.{tableName}.{fieldName}");
    }
}

static string ExtractContentAfterColon(string input)
{
    // Find the index of the colon
    int colonIndex = input.IndexOf(':');

    // If a colon is found, extract the text after it
    if (colonIndex != -1 && colonIndex < input.Length - 1)
    {
        int endIndex = input.IndexOf('#', colonIndex + 1); // Find the index of '#' after the colon
        if (endIndex != -1)
        {
            return input.Substring(colonIndex + 2, endIndex - colonIndex - 2).Trim();
        }
    }

    // If no colon is found or it's at the end of the string, return an empty string
    return string.Empty;
}

void OnFileChanged(object sender, FileSystemEventArgs e)
{
    // When the .sqlite file is updated, execute the GetLatestFieldValue method
    string? latestValue = DatabaseExtractor.GetLatestFieldValue(databasePath, tableName, fieldName, orderByField);
    if ((latestValue ?? "").EndsWith("#{user}:"))
    {
        // Process the response
        Console.WriteLine("Processing response:");
        Console.WriteLine("HERE I AM: " + latestValue);

        // Your additional code here...
    }
}