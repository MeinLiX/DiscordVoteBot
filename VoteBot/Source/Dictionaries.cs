using System.Collections.Generic;

using VoteBot.Source.Reactions_Handle;

namespace VoteBot.Source
{
    class Dictionaries
    {
        internal static Dictionary<string, IReactionHandle> VoteType__ADD_Reactions = new ()
        {
            ["#vote_s"] = new Vote_S__ADD_RH(),
            ["#vote_m"] = new Vote_M__ADD_RH(),
        };
    }
}
