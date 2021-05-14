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

        [JsonPropertyName("emoteChannelIds")]
        public ulong[] EmoteChannelIds { get; set; }
    }
}
