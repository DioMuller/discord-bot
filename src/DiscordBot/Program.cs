using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
           => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                var config = services.GetRequiredService<Config>();

                await client.LoginAsync(TokenType.Bot, config.Token);
                await client.StartAsync();

                await services.GetRequiredService<HandlerService>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private Config LoadConfig(string filename)
        {
            return JsonSerializer.Deserialize<Config>(filename);
        }

        private ServiceProvider ConfigureServices()
        {
            var collection = new ServiceCollection();

            // Add Config
            collection.AddSingleton<Config>(LoadConfig(File.ReadAllText("config.json")));

            // Add Discord Classes
            collection.AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>();
                
            // Add Our Services
            collection.AddSingleton<HandlerService>()
                .AddSingleton<EmoteService>()
                .BuildServiceProvider();

            return collection.BuildServiceProvider();
        }
    }
}
