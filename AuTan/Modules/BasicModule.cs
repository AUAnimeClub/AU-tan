using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AuTan.Modules;

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
        try
        {
            var helpMsg = await File.ReadAllTextAsync(Path.Join(AppDomain.CurrentDomain.BaseDirectory,
                "./resources/help/index.md"));
            var user = (SocketGuildUser)Context.User;
            if (user.Roles.Any(x => x.Name == "Committee"))
            {
                helpMsg += "\n\nAs you are a committee member, more details " +
                    "on these commands can be found on our Trello board: \n" +
                    "https://trello.com/c/cq0ditFg/290-bot-au-tan-feature-list-here";
            }
            await Context.User.SendMessageAsync(helpMsg);
            if (Context.Guild != null)
            {
                await Context.Message.AddReactionAsync(new Emoji("📬"));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}