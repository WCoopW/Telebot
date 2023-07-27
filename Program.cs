using Newtonsoft.Json.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.BotBuilder.Interfaces;
using TelegramBot.Schedule;

namespace TelegramBot
{

    internal class Program
    {
        static bool SearchFlight = false;
        static void Main(string[] args)
        {
            
            //var p = new Repo();
            //await p.ScheduleGet();
            //Console.ReadLine();
            var botClient = new TelegramBotClient("6131644600:AAHMvTuxwzSyAhuwVjl1y0OxEIb_i_TpghU");
            var cts = new CancellationTokenSource();
            botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            cancellationToken: cts.Token);
            Console.ReadLine();
        }



        private static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
           
            var message = update.Message;
            var callback = update.CallbackQuery;

            if (callback != null)
            {
                if (callback.Data == "LED")
                {
                    Message sentMessage = await client.SendTextMessageAsync(chatId: callback.From.Id,
                        text: "Укажите имя персонажа",
                        replyMarkup: new ForceReplyMarkup { Selective = true });
                }
               
            }
           
           
            if (message != null)
            {
                TimeSpan times = DateTime.UtcNow - update.Message.Date;
                if (times.TotalMinutes > 1)
                {
                    Console.WriteLine("skipping old update");
                    return;
                }
                if (message.ReplyToMessage != null && message.ReplyToMessage.Text.Contains("Укажите имя персонажа"))
                {
                    var b = new Repo();
                    var characters = b.ScheduleGet();
                    var name = message.Text;
                    var character = SearchPerson(name, characters);
                    if (character != null)
                    {
                        Message sentMessage = await client.SendPhotoAsync(
                        chatId: message.Chat.Id,
                        photo: InputFile.FromUri($"{character.image}"),
                        cancellationToken: token);
                        sentMessage = await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"Имя: {character.name}\nПол: {character.gender}\nСтатус: {character.status}\nМесто обитания: {character.origin.name}",
                            replyMarkup: MainButtons(),
                            cancellationToken: token);
                    }
                    else
                    {
                        Message sentMessage = await client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Такого персонажа нет. Хотите попробовать снова?",
                            replyMarkup: FailedSearchCharacter(),
                            cancellationToken: token);
                            
                    }
                   

                    return;
                }
                if (message.Text != null)
                {

                    await WorkWithTextCommand(client, message, token);
                    return;
                }
                if (message.Document != null) 
                {
                    Message sentMessage = await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Я не знаю что с этим делать",
                        cancellationToken: token);
                }
                else
                {
                    Message sentMessage = await client.SendTextMessageAsync(
                     chatId: message.Chat.Id,
                     text: "Выберите одну из доступных команд",
                     cancellationToken: token);

                }
            }
           



        }

        private static IReplyMarkup? FailedSearchCharacter()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
         {
             new KeyboardButton[] { "Да, ввести имя заново"},
             new KeyboardButton[] { "Нет, выйти в главное меню"},
            })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }

        private static Result SearchPerson(string? name, Task<Root> characters)
        {
            foreach(var character in characters.Result.results)
            {
                if (name == character.name) {
                    return character;
                }
               
            }
            return null;
        }

        private static async Task WorkWithTextCommand(ITelegramBotClient client, Message message, CancellationToken token)
        {
            if (message.Text.Contains("Да, ввести имя заново"))
            {
                Message sentMessage = await client.SendTextMessageAsync(chatId: message.Chat.Id,
                      text: "Укажите имя персонажа",
                      replyMarkup: new ForceReplyMarkup { Selective = true });
            }
            if (message.Text.Contains("Нет, выйти в главное меню")) 
            {
                Message sentMessage = await client.SendTextMessageAsync(
               chatId: message.Chat.Id,
               text: "Выберите действие",
               replyMarkup: MainButtons(),
               cancellationToken: token);
            }
            if (message.Text.ToLower().Contains("start"))
            {
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Выберите действие",
                replyMarkup: MainButtons(),
                cancellationToken: token);
            }
            if (message.Text.ToLower().Contains("узнать доступные рейсы"))
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                new []
                {
                 InlineKeyboardButton.WithCallbackData(text: "Поиск персонажа по имени", callbackData: "LED"),
                } 
                });
                Message sentMessage = await client.SendTextMessageAsync(
              chatId: message.Chat.Id,
              text: "Выберите что вы хотите сделать",
              replyMarkup: inlineKeyboard,
              cancellationToken: token
             
               );
                
            }
            if (message.Text.ToLower().Contains("купить билет"))
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                InlineKeyboardButton.WithUrl(
                 text: "Купить билет",
                url: "https://www.aviasales.ru/routes/led")
                 });
                Message sentMessage = await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Приобрести билет можно на официальном сайте",
                replyMarkup: inlineKeyboard,
                cancellationToken: token);
            }
            if (message.Text.ToLower().Contains("помощь"))
            {
                Message sentMessage = await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Чем могу помочь?",
                    replyMarkup: HelpButtons(),
                    cancellationToken: token);
            }
            if (message.Text.ToLower().Contains("что умеет бот"))
            {
                Message sentMessage = await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Данный бот позволяет узнать вам доступные билеты на самолет из города Санкт-Петербург и купить их." + 
                    "\nДля начала работы бота нажмите /start",
                    cancellationToken: token);

            }
            if (message.Text.ToLower().Contains("об авторе"))
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                InlineKeyboardButton.WithUrl(
                 text: "Яндекс расписание",
                url: "https://rasp.yandex.ru")
                 });
                Message sentMessage = await client.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: "Создатель бота @clarkecooper. " +
                                    "\nРасписание получено с помощью сервиса Яндекс.Расписание",
                                    replyMarkup: inlineKeyboard,
                                    cancellationToken: token);
            }
            if (message.Text.ToLower().Contains("главное меню"))
            {
                Message sentMessage = await client.SendTextMessageAsync(
                                   chatId: message.Chat.Id,
                                   text: "Выберите действие",
                                   replyMarkup: MainButtons(),
                                   cancellationToken: token);
            }
        }

        private static IReplyMarkup? HelpButtons()
        {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
             new KeyboardButton[] { "Что умеет бот?"},
             new KeyboardButton[] { "Об авторе" },
             new KeyboardButton[] { "Главное меню" }
            })
                {
                ResizeKeyboard = true
                };
        return replyKeyboardMarkup;
        }

        private static IReplyMarkup? MainButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
         new KeyboardButton[] { "Узнать доступные рейсы ✈️"},
         new KeyboardButton[] { "Купить билет 💸" },
         new KeyboardButton[] { "Помощь ❓" }
        })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;

        }



        private static Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exp, CancellationToken token)
        {
            throw new NotImplementedException();
        }

       
    }
}
    
