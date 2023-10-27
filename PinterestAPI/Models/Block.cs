using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Block
{
    public int BlockId { get; set; }

    public int? BlockingUserId { get; set; }

    public int? BlockedUserId { get; set; }

    public DateTime? BlockDate { get; set; }

    public virtual User? BlockedUser { get; set; }

    public virtual User? BlockingUser { get; set; }
}
