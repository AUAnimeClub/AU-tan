using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AuTan.Modules;

public class AnnouncementModule : ModuleBase<SocketCommandContext>
{
    [Command("announce")]
    public async Task Announce(ITextChannel channel, [Remainder] string content)
    {
        var user = (SocketGuildUser) Context.User;
        if (user.Roles.All(x => x.Name != "Committee"))
        {
            await ReplyAsync("You don't have permission to make announcements "+
                "(requires Committee role). ");
            return;
        }
        await channel.SendMessageAsync(content);
        await Context.Message.AddReactionAsync(new Emoji("📝"));
    }
    
    [Command("announce.edit")]
    public async Task Announce(string oldMessageUrl, [Remainder] string content)
    {
        var user = (SocketGuildUser) Context.User;
        if (user.Roles.All(x => x.Name != "Committee"))
        {
            await ReplyAsync("You don't have permission to edit announcements " +
                "(requires Committee role). ");
            return;
        }

        var msg = await Utils.MessageFromUrlAsync(oldMessageUrl, Context);
        await ((IUserMessage) msg).ModifyAsync(x => x.Content = content);
        await Context.Message.AddReactionAsync(new Emoji("📝"));
    }
}