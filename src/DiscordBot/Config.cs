using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBot
{
    public class Config
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("emoteChannelIds")]
        public List<ulong> EmoteChannelIds { get; set; }

        public void Save()
        {
            var text = JsonSerializer.Serialize<Config>(this);
            File.WriteAllText("config.json", text);
        }
    }
}
