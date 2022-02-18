using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AuTan.Modules;

[Group("color")]
public class ColorModule : ModuleBase<SocketCommandContext>
{
    private static readonly Regex ColorRegex = new Regex("^#(?:[0-9a-fA-F]{3}){1,2}$");

    private static async Task ClearColorRoles(IReadOnlyCollection<SocketRole> roles, IGuildUser user)
    {
        foreach (var roleId in user.RoleIds)
        {
            var role = roles.First(x => x.Id == roleId);
            if (ColorRegex.IsMatch(role.Name))
            {
                await user.RemoveRoleAsync(role);
            }
        } 
    }

    [Command]
    public async Task SetColor(string color)
    {
        var user = (IGuildUser) Context.User;

        if (color == "clear")
        {
            await ClearColorRoles(Context.Guild.Roles, user);
            await ReplyAsync("Removed the color roles!");
            return;
        }
        if (!ColorRegex.IsMatch(color))
        {
            await ReplyAsync("Not a valid color!");
            return;
        }
        var role = Context.Guild.Roles
            .FirstOrDefault(x => x.Name == color);
        if (role == null)
        {
            await ReplyAsync("Sorry, we don't have that color yet :(");
            return;
        }
        
        await ClearColorRoles(Context.Guild.Roles, user);
        await user.AddRoleAsync(role.Id);
        await ReplyAsync($"Alright, gave you the color {role.Name}!");
    }
}