using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class FollowBoard
{
    public int FollowBoardId { get; set; }

    public int UserId { get; set; }

    public int BoardId { get; set; }

    public virtual Board Board { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
