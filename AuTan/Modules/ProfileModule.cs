using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AuTan.Modules
{
    public class ProfileModule : ModuleBase<SocketCommandContext>
    {
        [Command("me")]
        public async Task Me()
        {
            var embed = new EmbedBuilder()
                .WithTitle($"{Context.User.Username}#{Context.User.Discriminator}")
                .WithThumbnailUrl(Context.User.GetAvatarUrl())
                .WithFields(
                    new EmbedFieldBuilder().WithName("Level").WithValue(42),
                    new EmbedFieldBuilder().WithName("XP").WithValue("32/69420")
                );
            await ReplyAsync(embed: embed.Build());
        }
    }
}