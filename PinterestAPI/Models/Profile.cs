using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public string? Name { get; set; }

    public string? Lastname { get; set; }

    public string? Information { get; set; }

    public string? WebSite { get; set; }

    public string UserName { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Email { get; set; }

    public byte[]? ProfilePhoto { get; set; }

    public bool? Privacy { get; set; }

    public virtual User User { get; set; } = null!;
}
