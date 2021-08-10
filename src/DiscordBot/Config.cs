using DiscordBot.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBot
{
    public class Feed
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("feed")]
        public string Uri { get; set; }

        [JsonPropertyName("channel")]
        public ulong Channel { get; set; }

        [JsonPropertyName("lastUpdate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime LastUpdate { get; set; }
    }

    public class Config
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("emoteChannelIds")]
        public List<ulong> EmoteChannelIds { get; set; }

        [JsonPropertyName("useNewsFeed")]
        public bool UseNewsFeed { get; set; }

        [JsonPropertyName("newsFeedUpdate"), JsonConverter(typeof(TimespanConverter))]
        public TimeSpan NewsFeedUpdate { get; set; }

        [JsonPropertyName("feeds")]
        public List<Feed> Feeds { get; set; } = new List<Feed>();

        public void Save()
        {
            var text = JsonSerializer.Serialize<Config>(this);
            File.WriteAllText("config.json", text);
        }
    }
}
