using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Board
{
    public int BoardId { get; set; }

    public string BoardName { get; set; } = null!;

    public string? BoardDescription { get; set; }

    public DateTime Date { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<FollowBoard> FollowBoards { get; set; } = new List<FollowBoard>();

    public virtual ICollection<PinBoardAssociation> PinBoardAssociations { get; set; } = new List<PinBoardAssociation>();

    public virtual User? User { get; set; }
}
