using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoteBot.Source.Reactions_Handle
{
    class Vote_S__ADD_RH : IReactionHandle
    {
        public Task Invoke(IMessage message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            try
            {
                Parallel.ForEach(message.Reactions,
                async msg_reaction =>
                {
                    bool authorPlacedReaction = false;
                    Parallel.ForEach((await message?.GetReactionUsersAsync(msg_reaction.Key, 10000).FlattenAsync()) as ICollection<IUser>,
                    (user_reaction, state) =>
                    {
                        if (user_reaction.Id == message.Author.Id)
                        {
                            authorPlacedReaction = true;
                        }

                        if (msg_reaction.Key.Name == reaction.Emote.Name && msg_reaction.Value.ReactionCount > 1 && !user_reaction.IsBot)
                        {
                            state.Break();
                        }

                        if (user_reaction.Id == reaction.UserId)
                        {
                            message.RemoveReactionAsync(msg_reaction.Key, reaction.UserId);
                        }
                    });

                    if (!authorPlacedReaction)
                    {
                        await message.RemoveAllReactionsForEmoteAsync(reaction.Emote);
                    }
                });
            }
            catch { throw; }
            return Task.CompletedTask;
        }
    }
}
