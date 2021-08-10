using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;

namespace DiscordBot.Services
{
    public class FeedService
    {
        private readonly DiscordSocketClient _client;
        private Config _config;

        public FeedService(DiscordSocketClient client,  Config config)
        {
            _client = client;
            _config = config;

            Run();
        }

        private void Run()
        {
            while(_config.UseNewsFeed)
            {
                Thread.Sleep(_config.NewsFeedUpdate);

                foreach (var feed in _config.Feeds)
                {
                    var channel = _client.GetChannel(feed.Channel) as ISocketMessageChannel;

                    if (channel != null)
                    {
                        XmlReader reader = XmlReader.Create(feed.Uri);
                        SyndicationFeed feedInfo = SyndicationFeed.Load(reader);
                        reader.Close();

                        foreach (var item in feedInfo.Items)
                        {
                            if (item.PublishDate > feed.LastUpdate)
                            {
                                StringBuilder builder = new StringBuilder();
                                
                                builder.AppendLine($"**{item.Title.Text}**").AppendLine();

                                if (item.Summary != null)
                                    builder.AppendLine(item.Summary.Text).AppendLine();

                                if ( item.Links.Count > 0 )
                                    builder.AppendLine($"*See more at:* <{item.Links[0].Uri.AbsoluteUri}>");

                                channel.SendMessageAsync(builder.ToString());
                            }
                        }
                    }

                    feed.LastUpdate = DateTime.Now;
                }

                _config.Save();
            }
        }
    }
}
