using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class BotModule : ModuleBase<SocketCommandContext>
    {
        public BotService BotService { get; set; }

        public Config Config { get; set; }

        [Command("name")]
        public Task NameAsync([Remainder] string name)
            => BotService.ChangeName(name);

        [Command("add_channel")]
        public async Task AddChannel(SocketChannel channel)
        {
            if (!Config.EmoteChannelIds.Contains(channel.Id))
            {
                Config.EmoteChannelIds.Add(channel.Id);
                Config.Save();
                await ReplyAsync($"Added Channel {channel.Id}");
            }
            else
            {
                await ReplyAsync($"Channel {channel.Id} was already on the channel list.");
            }
        }

        [Command("remove_channel")]
        public async Task RemoveChannel(SocketChannel channel)
        {
            if (Config.EmoteChannelIds.Contains(channel.Id))
            {
                Config.EmoteChannelIds.Remove(channel.Id);
                Config.Save();
                await ReplyAsync($"Removed Channel {channel.Id}");
            }
            else
            {
                await ReplyAsync($"Channel {channel.Id} was not on the channel list.");
            }
        }
    }
}
