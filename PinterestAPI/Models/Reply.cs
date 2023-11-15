using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Reply
{
    public int ReplyId { get; set; }

    public string Reply1 { get; set; } = null!;

    public int CommentId { get; set; }

    public int UserId { get; set; }

    public virtual Comment Comment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
