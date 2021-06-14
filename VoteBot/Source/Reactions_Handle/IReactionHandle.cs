using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace VoteBot.Source.Reactions_Handle
{
    interface IReactionHandle
    {
        public Task Invoke(IMessage message, ISocketMessageChannel channel, SocketReaction reaction);
    }
}
