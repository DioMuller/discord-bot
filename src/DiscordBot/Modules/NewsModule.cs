using Discord.Commands;
using DiscordBot.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Modules
{
    public class NewsModule : ModuleBase<SocketCommandContext>
    {
        public FeedService FeedService { get; set; }

    }
}
