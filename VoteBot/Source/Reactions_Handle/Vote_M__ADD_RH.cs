using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoteBot.Source.Reactions_Handle
{
    class Vote_M__ADD_RH : IReactionHandle
    {
        public async Task Invoke(IMessage message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            try
            {
                bool authorPlacedReaction = false;
                Parallel.ForEach((await message?.GetReactionUsersAsync(reaction.Emote, 10000).FlattenAsync()) as ICollection<IUser>,
                (user_reaction, state) =>
                {
                    if (user_reaction.Id == message.Author.Id)
                    {
                        authorPlacedReaction = true;
                        state.Stop();
                    }
                });

                if (!authorPlacedReaction)
                {
                    await message.RemoveAllReactionsForEmoteAsync(reaction.Emote);
                }
            }
            catch { throw; }
        }
    }
}
