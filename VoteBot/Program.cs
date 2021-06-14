using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

using VoteBot.Source;

namespace VoteBot
{
    class Program
    {
        static async Task Main(string[] args) => await new Program().RunBotAsync();

        private DiscordSocketClient _client;

        public async Task RunBotAsync()
        {
            _client = new();

            RegisterPackagesAsync();

            await _client.LoginAsync(TokenType.Bot, Config.Config.Get().Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public void RegisterPackagesAsync()
        {
            _client.Log += Client_Log;
            _client.ReactionAdded += HandleReactionAddAsync;
        }

        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task HandleReactionAddAsync(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            try
            {
                IMessage msg = await channel.GetMessageAsync(message.Id);
                Task<IUser> userTask = channel.GetUserAsync(reaction.UserId);
                foreach (var voteType in Dictionaries.VoteType__ADD_Reactions.Where(voteType => msg.ToString().Contains(voteType.Key)))
                {
                    SocketGuildUser user = (await userTask) as SocketGuildUser;
                    if (user.GuildPermissions.Administrator || user.Id == msg.Author.Id)
                        return;

                    await voteType.Value.Invoke(msg, channel, reaction);
                }
            }
            catch { throw; }
        }
    }
}
