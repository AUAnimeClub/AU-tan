using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AuTan;

public static class Utils : object
{
    /** 
     * <summary>
     * Converts a mesage URL to an IMessage object. 
     * </summary>
     * <param name="url">URL of the message</param>
     * <param name="context">SocketCommandContext used to get the IMessage object</param>
     */
    public static async Task<IMessage> MessageFromUrlAsync(
        string url, SocketCommandContext context)
    {
        var msgUri = url.Split("/");
        ulong channel_id = ulong.Parse(msgUri[^2]);
        ulong message_id = ulong.Parse(msgUri[^1]);
        ISocketMessageChannel channel = ((ISocketMessageChannel)
            context.Guild.GetChannel(channel_id));
        IMessage mesage = await channel.GetMessageAsync(message_id);
        return mesage;
    }

    /**
     * <summary>
     * Converts a collections object to an indented (4 spaces) JSON string. 
     * </summary>
     */
    public static string SerializeToJson<T>(T value)
    {
        StringBuilder sb = new StringBuilder(256);
        StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);

        var jsonSerializer = JsonSerializer.CreateDefault();
        using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
        {
            jsonWriter.Formatting = Formatting.Indented;
            jsonWriter.IndentChar = ' ';
            jsonWriter.Indentation = 4;

            jsonSerializer.Serialize(jsonWriter, value, typeof(T));
        }

        return sw.ToString();
    }
}
