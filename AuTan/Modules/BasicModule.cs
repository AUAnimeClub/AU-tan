using System.Threading.Tasks;
using Discord.Commands;

namespace AuTan.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong!");
        }
    }
}