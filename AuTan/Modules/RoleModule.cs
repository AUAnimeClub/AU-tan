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
        public async Task AddRollAsyync(params IRole[] roles)
        {
            IGuildUser user = Context.User as IGuildUser;
            if (user.GuildPermissions.ManageRoles)
            {
                ulong[] role_ids = new ulong[roles.Length];
                for (int i = 0; i < roles.Length; i++)
                {
                    role_ids[i] = roles[i].Id;
                }
                await user.AddRolesAsync(role_ids);
            }
        }

        [Command("remove")]
        [Summary("Remove a role(s)")]
        public async Task RemoveRollAsyync(params IRole[] roles)
        {
            IGuildUser user = Context.User as IGuildUser;
            if (user.GuildPermissions.ManageRoles)
            {
                ulong[] role_ids = new ulong[roles.Length];
                for (int i = 0; i < roles.Length; i++)
                { 
            
                    role_ids[i] = roles[i].Id;
                }
                await user.RemoveRolesAsync(role_ids);
            }
        }
    }
}
