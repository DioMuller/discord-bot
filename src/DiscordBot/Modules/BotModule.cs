using Discord.Commands;
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

        [Command("name")]
        public Task NameAsync([Remainder] string name)
            => BotService.ChangeName(name);
    }
}
