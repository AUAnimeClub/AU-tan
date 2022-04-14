using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using dotenv.net;
using Microsoft.Extensions.DependencyInjection;

namespace AuTan;

public static class Program
{
    private static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();
        return services.BuildServiceProvider();
    }
        
    private static async Task Main(string[] args)
    {
        DotEnv.Load(new DotEnvOptions(probeLevelsToSearch: 5, probeForEnv:true));

        var client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All,
        });
        client.Log += message =>
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        };

        await new CommandHandler(client, new CommandService(), BuildServices())
            .InstallCommandsAsync();

        await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
        await client.StartAsync();
        await client.SetGameAsync("something");

        await Task.Delay(-1);
    }
}