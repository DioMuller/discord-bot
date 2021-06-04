using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Discord;
using System.Linq;
using System.IO;

namespace DiscordBot.Services
{
    public class HandlerService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly EmoteService _emote;
        private readonly Config _config;
        private readonly IServiceProvider _services;

        public HandlerService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _emote = services.GetRequiredService<EmoteService>();
            _config = services.GetRequiredService<Config>();
            _services = services;

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;

            if (_config.EmoteChannelIds.Contains(message.Channel.Id))
            {
                if (_emote.HasEmote(message.Content))
                {
                    var emotes = _emote.GetEmotes(message.Content);

                    foreach (var emote in emotes)
                        await message.Channel.SendMessageAsync(_emote.Enlarge(emote));

                    return;
                }

                if (message.Attachments.Count > 0)
                {
                    foreach (var attachment in message.Attachments)
                    {
                        var img = _emote.DownloadAndResize(attachment.ProxyUrl, 48);
                        if (img != null)
                        {
                            await message.Channel.SendFileAsync(img, "Big");
                            File.Delete(img);
                        }


                        img = _emote.DownloadAndResize(message.Content, 24);
                        if (img != null)
                        {
                            await message.Channel.SendFileAsync(img, "Small");
                            File.Delete(img);
                        }
                    }
                }

                if( Uri.IsWellFormedUriString(message.Content, UriKind.Absolute) )
                {
                    var img = _emote.DownloadAndResize(message.Content, 48);
                    if (img != null)
                    {
                        await message.Channel.SendFileAsync(img, "Big");
                        File.Delete(img);
                    }

                    img = _emote.DownloadAndResize(message.Content, 24);
                    if (img != null)
                    {
                        await message.Channel.SendFileAsync(img, "Small");
                        File.Delete(img);
                    }
                }
            }

            // Perform prefix check. You may want to replace this with
            // (!message.HasCharPrefix('!', ref argPos))
            // for a more traditional command format like !help.
            if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos) && !message.HasCharPrefix('!', ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await _commands.ExecuteAsync(context, argPos, _services);
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
        }
    }
}
