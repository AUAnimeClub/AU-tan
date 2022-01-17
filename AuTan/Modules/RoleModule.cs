using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AuTan.Modules;

[Group("role")]
public class RoleModule : ModuleBase<SocketCommandContext>
{
    [Command("add")]
    [Summary("Add a role(s)")]
    public async Task AddRoleAsync(params IRole[] roles)
    {
        var user = (IGuildUser) Context.User;
        if (user.GuildPermissions.ManageRoles)
        {
            var roleIds = new ulong[roles.Length];
            for (var i = 0; i < roles.Length; i++)
            {
                roleIds[i] = roles[i].Id;
            }
            await user.AddRolesAsync(roleIds);
        }
    }

    [Command("remove")]
    [Summary("Remove a role(s)")]
    public async Task RemoveRoleAsync(params IRole[] roles)
    {
        var user = (IGuildUser) Context.User;
        if (user.GuildPermissions.ManageRoles)
        {
            var roleIds = new ulong[roles.Length];
            for (var i = 0; i < roles.Length; i++)
            {
                roleIds[i] = roles[i].Id;
            }
            await user.RemoveRolesAsync(roleIds);
        }
    }
}