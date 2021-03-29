using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class BotService
    {
        private DiscordSocketClient _client;
        public BotService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task ChangeName(string name)
        {
            foreach (var guild in _client.Guilds)
            {
                var user = guild.GetUser(_client.CurrentUser.Id);
                await user.ModifyAsync( x => x.Nickname = name );
            }                    
        }
    }
}
