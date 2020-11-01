using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Core.Extensions
{
    // Using one extensions class for now to keep things simple, will split if needed
    public static class GeneralExtensions
    {
        public static async Task DeleteAfterTime(this IMessage message, TimeSpan delay)
        {
            await Task.Delay(delay);

            await message.DeleteAsync();
        }

        public static void Forget(this Task task)
        {
            task.Forget(DefaultExceptionHandler);
        }

        public static async void Forget(this Task task, Action<Exception> exceptionHandler)
        {
            if (exceptionHandler == null)
            {
                throw new ArgumentNullException(nameof(exceptionHandler));
            }

            try
            {
                await task;
            }
            catch (Exception e)
            {
                exceptionHandler(e);
            }
        }

        private static void DefaultExceptionHandler(Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}
