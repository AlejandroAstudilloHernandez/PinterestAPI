using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class PinBoardAssociation
{
    public int PinBoardId { get; set; }

    public int PinId { get; set; }

    public int BoardId { get; set; }

    public virtual Board Board { get; set; } = null!;

    public virtual Pin Pin { get; set; } = null!;
}
