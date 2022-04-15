using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuTan.Modules;

public class ReactionModule : ModuleBase<SocketCommandContext>
{
    /**
     * NOTE: Usage with messages that have many reactions may cause rate limiting 
     * due to retrieving many user objects within a short time period. 
     * If this occurs Au-tan may freeze and stop responding to commands. 
     * Note this in the detailed help when it is implemented. 
     */

    private static readonly Int32 limit = 5000;

    [Command("reactions.get")]
    [Summary("Get all the users who have reacted to a message with the specified emote")]
    public async Task AttachReactionsAsync(
        string emote_str, string message_link)
    {
        //check the user who called the command has the required permissions
        var caller = (SocketGuildUser)Context.User;
        if (caller.Roles.All(x => x.Name != "Mod"))
        {
            await ReplyAsync(
                "You don't have permission to use reaction utilities " +
                "(requires Mod role). ");
            return;
        }

        //Populate the dictionary used to generate the reply JSON. 
        IMessage message = await Utils.MessageFromUrlAsync(message_link, Context);
        var reactions = await GetReactionsAsync(emote_str, message_link);
        Dictionary<string, dynamic> reactions_dict = new();
        reactions_dict.Add(key: "Message URL", value: message_link);
        reactions_dict.Add(key: "Message content", value: message.Content);
        reactions_dict.Add(key: "Emote", value: emote_str);
        reactions_dict.Add(key: "Reactions with emote", value: reactions.Count());
        reactions_dict.Add(key: "Retrieval limit", value: limit);
        List<string> user_list = new();
        foreach (IUser user in reactions)
        {
            IGuildUser guild_user = Context.Guild.GetUser(user.Id);
            if (guild_user == null)
            {
                continue;
            }
            string nickname = guild_user.Nickname != null ? guild_user.Nickname : "";
            string user_str = string.Format("{0}, {1}",
                user.ToString(), nickname);
            user_list.Add(user_str);
        }
        reactions_dict.Add(
            key: "Reacted users (global username, guild nickname)",
            value: user_list);

        //generate a temporary JSON file to attach to the reply
        string json_str = Utils.SerializeToJson(reactions_dict);
        string file_name = @"reactions.json";
        if (File.Exists(file_name))
            File.Delete(file_name);
        using (FileStream fs = File.Create(file_name))
        {
            Byte[] contents = new UTF8Encoding(true).GetBytes(json_str);
            fs.Write(contents, 0, contents.Length);
        }
        string reply_str = LimitReachedWarning(reactions, emote_str);
        await Context.Channel.SendFileAsync(filePath: file_name,
            text: reply_str);
        if (File.Exists(file_name))
            File.Delete(file_name);
    }

    [Command("reactions.has_reacted")]
    [Summary("Checks if a user (case sensitive) has reacted to the message " +
        "with the specified emote")]
    public async Task HasReactedAsync(
        string emote_str, string user_str, string message_link)
    {
        //If the requested user was mentioned, parse it as a user object
        IGuildUser user_obj = null;
        if (Regex.IsMatch(user_str, @"^\<\@\!?.*\>$", RegexOptions.Singleline))
        {
            user_str = Regex.Replace(user_str, @"^\<\@\!?|\>$", "");
            user_obj = Context.Guild.GetUser(ulong.Parse(user_str));
            user_str = user_obj.Nickname != null ? user_obj.Nickname: user_obj.Username;
        }

        //check the user who called the command has the required permissions
        var caller = (SocketGuildUser)Context.User;
        if (caller.Roles.All(x => x.Name != "Mod"))
        {
            await ReplyAsync(
                "You don't have permission to use reaction utilities " +
                "(requires Mod role). ");
            return;
        }

        //set the default reply which will be sent if no matching users are found
        var reactions = await GetReactionsAsync(emote_str, message_link);
        string reply_str = String.Format(
            "Reaction {0} by user \"{1}\" **was not** found on the linked message. " +
            "This does not mean they haven't reacted to the message. Just that " +
            "the username provided was not found in the set of reactions. " +
            "\n**NOTE:** Recommend checking the casing of the username specified " +
            "or using \"reactions.get\" and manually searching for the user. ",
            emote_str, user_obj != null ? user_obj.Mention : user_str);

        int count = 0;
        foreach (IUser user in reactions)
        {
            IGuildUser guild_user = Context.Guild.GetUser(user.Id);
            string username = user.ToString();
            string nickname = guild_user.Nickname;
            //Use string matching to find all potential matches
            if (username.Contains(user_str) ||
                nickname != null && nickname.Contains(user_str))
            {
                //If the requested user was a mention, ignore all non exact matches
                if (user_obj != null && !user_obj.Equals(guild_user))
                {
                    continue;
                }

                
                if (count == 0)
                {
                    reply_str = string.Format(
                        "User \"{0}\" **has** reacted to the linked message " +
                        "with emote {1} by matching the following users: ",
                        user_obj != null ? user_obj.Mention : user_str, emote_str);
                }

                if (nickname == null)
                {
                    reply_str += string.Format(
                        "\n\t + global username: \"**{0}**\" " +
                        "(guild nickname not set). ", user.ToString());
                }
                else
                {
                    reply_str += string.Format(
                        "\n\t + global username: \"**{0}**\", or guild nickname: " +
                        "\"**{1}**\". ", user.ToString(), guild_user.Nickname);
                }
                count++;
            }
        }
        reply_str += LimitReachedWarning(reactions, emote_str);
        await Context.Channel.SendMessageAsync(reply_str);
    }

    public async Task<IEnumerable<IUser>> GetReactionsAsync(
       string emote_str, string message_link)
    {
        IEmote emote = Regex.IsMatch(emote_str, @"^<:.*:>$", RegexOptions.Singleline) ?
            Emote.Parse(emote_str) : new Emoji(emote_str);
        IMessage message = await Utils.MessageFromUrlAsync(message_link, Context);
        IEnumerable<IUser> reactions =
            await message.GetReactionUsersAsync(emote, limit).FlattenAsync();
        return reactions;
    }

    public string LimitReachedWarning(IEnumerable<IUser> reactions, string emote_str)
    {
        if (reactions.Count() == limit)
        {
            return string.Format(
                "\n**WARNING:** Limit on number of user reactions ({0}) that can " +
                "be retrieved was reached. As a result, the list of reactions " +
                "likely does not include all the {1} reactions to the message. ",
                limit, emote_str);
        }
        return "";
    }
}
