using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Services
{
    public class EmoteService
    {
        public string Enlarge(string emoteText)
        {
            try
            {
                var emote = Emote.Parse(emoteText);

                return emote.Url;
            }
            catch
            {
                return null;
            }

        }

        public bool IsEmote(string text)
        {
            Emote emote;

            return Emote.TryParse(text, out emote);
        }
    }
}
