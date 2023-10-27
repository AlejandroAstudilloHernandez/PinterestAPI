using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Reaction
{
    public int ReactionId { get; set; }

    public int? GoodIdeaReaction { get; set; }

    public int? LoveReaction { get; set; }

    public int? ThanksReaction { get; set; }

    public int? WowReaction { get; set; }

    public int? HahaReaction { get; set; }

    public int PinId { get; set; }

    public virtual Pin Pin { get; set; } = null!;
}
