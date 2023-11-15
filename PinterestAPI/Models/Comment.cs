using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Comment1 { get; set; } = null!;

    public int PinId { get; set; }

    public int UserId { get; set; }

    public virtual Pin Pin { get; set; } = null!;

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    public virtual User User { get; set; } = null!;
}
