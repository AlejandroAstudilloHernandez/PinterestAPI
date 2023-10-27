using System;
using System.Collections.Generic;

namespace PinterestAPI.Models;

public partial class Pin
{
    public int PinId { get; set; }

    public byte[] Image { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? AltText { get; set; }

    public string? Link { get; set; }

    public int UserId { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime Date { get; set; }

    public bool? SensitiveContent { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<PinBoardAssociation> PinBoardAssociations { get; set; } = new List<PinBoardAssociation>();

    public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public virtual ICollection<Saved> Saveds { get; set; } = new List<Saved>();

    public virtual User User { get; set; } = null!;
}
