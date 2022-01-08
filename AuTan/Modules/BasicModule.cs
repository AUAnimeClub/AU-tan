using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
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
        
        [Command("help")]
        public async Task Help()
        {
            var helpMsg = await File.ReadAllTextAsync("../../../../help/index.md");
            await Context.User.SendMessageAsync(helpMsg);
            if (Context.Guild != null)
            {
                await Context.Message.AddReactionAsync(new Emoji("📬"));
            }
        }
    }
}