using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DiscordBot
{
    public class Config
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("emoteChannelId")]
        public ulong EmoteChannelId { get; set; }
    }
}
