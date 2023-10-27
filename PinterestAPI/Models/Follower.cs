using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Follower
{
    public int FollowerId { get; set; }

    public int? UserFollowerId { get; set; }

    public int? UserFollowingId { get; set; }

    public virtual User? UserFollower { get; set; }

    public virtual User? UserFollowing { get; set; }
}
