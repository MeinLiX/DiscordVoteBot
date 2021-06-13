using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try { 
            var msg = await channel.GetMessageAsync(message.Id);

            if (!msg.ToString().Contains("#vote"))
                return;

            var user = (await channel.GetUserAsync(reaction.UserId)) as SocketGuildUser;
            if (user.GuildPermissions.Administrator || user.Id == msg.Author.Id)
                return;

            foreach (var msg_reaction in msg.Reactions)
            {
                var usersReacted = (await msg?.GetReactionUsersAsync(msg_reaction.Key, 1000).FlattenAsync()) as ICollection<IUser>;
                foreach (var user_reaction in usersReacted)
                {

                    if (msg_reaction.Key.Name == reaction.Emote.Name && usersReacted.Count > 1 && !user_reaction.IsBot)
                        break;

                    if (user_reaction.Id == reaction.UserId)
                        await msg.RemoveReactionAsync(msg_reaction.Key, reaction.UserId);
                }
            }
            }
            catch { throw; }

            return;
        }
    }
}
