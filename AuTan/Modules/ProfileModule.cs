using System;
using System.IO;
using System.Threading.Tasks;
using AuTan.ImageProcessing;
using Discord;
using Discord.Commands;
using SixLabors.ImageSharp;

namespace AuTan.Modules;

public class ProfileModule : ModuleBase<SocketCommandContext>
{

    [Command("me")]
    public async Task Me()
    {
        try
        {
            using (Context.Channel.EnterTypingState())
            {
                using var img = Card.GetCardImage(Context.User.Username, 100);
                await using var ms = new MemoryStream();
                await img.SaveAsPngAsync(ms);
                ms.Position = 0;
                await Context.Channel.SendFileAsync(ms, "card.png");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}