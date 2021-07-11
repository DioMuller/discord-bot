using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class BotModule : ModuleBase<SocketCommandContext>
    {
        public BotService BotService { get; set; }
        public DiscordSocketClient Discord { get; set; }
        public Config Config { get; set; }

        public BotModule()
        {
        }

        [Command("name")]
        public Task NameAsync([Remainder] string name)
            => BotService.ChangeName(name);

        [Command("add_channel")]
        public async Task AddChannel(SocketChannel channel)
        {
            if (Context.Guild.Owner.Id != Context.User.Id && Context.User.Id != 231024739425058816) return;
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
            if (Context.Guild.Owner.Id != Context.User.Id && Context.User.Id != 231024739425058816) return;
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

        [Command("cache_users")]
        public async Task Cache()
        {
            await ReplyAsync("Starting User Cache...");
            await Discord.DownloadUsersAsync(new[] { Context.Guild });
            await ReplyAsync("Finished User Cache!");
        }

        [Command("avatar")]
        public async Task Avatar(ulong uid = 0)
        {
            if (uid == 0) uid = Context.User.Id;
            var user = Discord.GetUser(uid);

            if (user != null)
                await ReplyAsync(user.GetAvatarUrl());
            else
                await ReplyAsync("No.");
        }

        [Command("avatar")]
        public async Task Avatar(SocketUser user = null)
        {
            if (user == null) user = Context.User;

            await ReplyAsync(user.GetAvatarUrl());
        }


    }
}
