using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PinterestAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public virtual ICollection<Block> BlockBlockedUsers { get; set; } = new List<Block>();

    public virtual ICollection<Block> BlockBlockingUsers { get; set; } = new List<Block>();

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<FollowBoard> FollowBoards { get; set; } = new List<FollowBoard>();

    public virtual ICollection<Follower> FollowerUserFollowers { get; set; } = new List<Follower>();

    public virtual ICollection<Follower> FollowerUserFollowings { get; set; } = new List<Follower>();

    public virtual ICollection<Pin> Pins { get; set; } = new List<Pin>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    public virtual ICollection<Saved> Saveds { get; set; } = new List<Saved>();
}

public class Encrypt
{
    public static string GetSHA256(string str)
    {
        SHA256 sha256 = SHA256Managed.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] stream = null;
        StringBuilder sb = new StringBuilder();
        stream = sha256.ComputeHash(encoding.GetBytes(str));
        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
        return sb.ToString();
    }
}