using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace AuTan.Modules
{
    [Group("role")]
    public class RoleModule : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [Summary("Add a role(s)")]
        public async Task AddRollAsync(params IRole[] roles)
        {
            IGuildUser user = Context.User as IGuildUser;
            if (user.GuildPermissions.ManageRoles)
            {
                ulong[] roleIds = new ulong[roles.Length];
                for (int i = 0; i < roles.Length; i++)
                {
                    roleIds[i] = roles[i].Id;
                }
                await user.AddRolesAsync(roleIds);
            }
        }

        [Command("remove")]
        [Summary("Remove a role(s)")]
        public async Task RemoveRollAsync(params IRole[] roles)
        {
            IGuildUser user = Context.User as IGuildUser;
            if (user.GuildPermissions.ManageRoles)
            {
                ulong[] roleIds = new ulong[roles.Length];
                for (int i = 0; i < roles.Length; i++)
                {
                    roleIds[i] = roles[i].Id;
                }
                await user.RemoveRolesAsync(roleIds);
            }
        }
    }
}