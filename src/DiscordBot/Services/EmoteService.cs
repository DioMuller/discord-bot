using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace DiscordBot.Services
{
    public class EmoteService
    {
        readonly Regex _regex = new Regex(@"<[a]?:\w+:[0-9]+>");

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

        public bool HasEmote(string text) =>
            _regex.IsMatch(text);

        public List<string> GetEmotes(string text)
        {
            var matches = _regex.Matches(text);

            return matches.Select(m => m.ToString()).Where(s => IsEmote(s)).ToList();
        }
    }
}
