using System;
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
            await ReplyAsync(
                "...You're not a committee member, why are you trying to make an announcement?");
            return;
        }
        await channel.SendMessageAsync(content);
    }

    private async Task<IMessage> MessageFromUrl(string url)
    {
        var msgUri = url.Split("/");
        return await ((ISocketMessageChannel) Context.Guild
                .GetChannel(ulong.Parse(msgUri[^2])))
                .GetMessageAsync(ulong.Parse(msgUri[^1]));
    }
    
    [Command("announce.edit")]
    public async Task Announce(string oldMessageUrl, [Remainder] string content)
    {
        var user = (SocketGuildUser) Context.User;
        if (user.Roles.All(x => x.Name != "Committee"))
        {
            await ReplyAsync(
                "...You're not a committee member, why are you trying to make an announcement?");
            return;
        }

        var msg = await MessageFromUrl(oldMessageUrl);
        await ((IUserMessage) msg).ModifyAsync(x => x.Content = content);
        await Context.Message.AddReactionAsync(new Emoji("📝"));
    }
}