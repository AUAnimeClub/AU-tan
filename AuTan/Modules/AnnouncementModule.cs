using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AuTan.Modules
{
    public class AnnouncementModule : ModuleBase<SocketCommandContext>
    {
        [Command("announce")]
        public async Task Announce(ITextChannel channel, [Remainder] string content)
        {
            var user = (SocketGuildUser) Context.User;
            if (user.Roles.All(x => x.Name != "Committee"))
            {
                await ReplyAsync(
                    "...You're not a committee member, why are you trying to make an announcement?");
                return;
            }
            await channel.SendMessageAsync(content);
        }
    }
}