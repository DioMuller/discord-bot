using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

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


        public string DownloadAndResize(string uri, int size)
        {
            var validFormats = new string[] { "png", "jpg", "jpeg", "gif" };

            if (validFormats.Any(f => uri.EndsWith(f)))
            {
                var image = DownloadImage(uri);
                var resized = Resize(image, size);
                var name = DateTime.Now.ToString("yyyyMMddhhmmss");
                var filename = $"{name}.png";

                resized.Save(filename);

                return filename;
            }

            return null;
        }

        private Bitmap DownloadImage(string uri)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(uri);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();
            return new Bitmap(responseStream);
        }

        private Bitmap Resize(Bitmap bmp, int size)
        {
            var originalWidth = bmp.Width;
            var originalHeight = bmp.Height;
            int newWidth = size, newHeight = size;
            double ratio = 1;

            if(originalWidth > originalHeight)
            {
                ratio = (double)originalHeight / originalWidth;
                newHeight = (int)(newHeight * ratio);
            }
            else if(originalHeight > originalWidth)
            {
                ratio = (double)originalWidth / originalHeight;
                newWidth = (int) (newWidth * ratio);
            }

            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var destImage = new Bitmap(newWidth, newHeight);

            destImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(bmp, destRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
