using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AuTan.Modules
{
    public class AnnouncementModule : ModuleBase<SocketCommandContext>
    {
        [Command("announce")]
        public async Task Announce(ITextChannel channel, [Remainder] string content)
        {
            await channel.SendMessageAsync(content);
        }
    }
}