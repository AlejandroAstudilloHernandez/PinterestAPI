using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Saved
{
    public int SavedId { get; set; }

    public int UserId { get; set; }

    public int PinId { get; set; }

    public virtual Pin Pin { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
