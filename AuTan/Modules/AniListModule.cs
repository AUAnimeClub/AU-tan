using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Anilist4Net;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace AuTan.Modules
{
    public class AniListModule : ModuleBase<SocketCommandContext>
    {
        private readonly Client _anilist;

        public AniListModule()
        {
            _anilist = new Client();
        }
        
        public static string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
        
        [Command("anime")]
        public async Task Anime([Remainder] string title)
        {
            using (Context.Channel.EnterTypingState())
            {
                var media = await _anilist.GetMediaBySearch(title);
                Console.WriteLine(JsonConvert.SerializeObject(media!.ExternalLinks));
                var embed = new EmbedBuilder().WithTitle(media!.EnglishTitle)
                    .WithUrl(media.SiteUrl)
                    .WithDescription(Truncate(
                        Regex.Replace(HttpUtility.HtmlDecode(media.DescriptionHtml), 
                            "<.*?>", string.Empty), 500))
                    .WithImageUrl($"https://img.anili.st/media/{media.Id}")
                    .WithFooter($"{media.Format} | Data provided by AniList");
                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}