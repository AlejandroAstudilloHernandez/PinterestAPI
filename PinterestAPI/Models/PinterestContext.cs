using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PinterestAPI.Models;

public partial class PinterestContext : DbContext
{
    public PinterestContext()
    {
    }

    public PinterestContext(DbContextOptions<PinterestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<Pin> Pins { get; set; }

    public virtual DbSet<PinBoardAssociation> PinBoardAssociations { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Reaction> Reactions { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Saved> Saveds { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-AOHL8UE\\SQLEXPRESS;Database=Pinterest;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.BlockId).HasName("PK__Blocks__144215F1A00BAE42");

            entity.Property(e => e.BlockDate).HasColumnType("datetime");

            entity.HasOne(d => d.BlockedUser).WithMany(p => p.BlockBlockedUsers)
                .HasForeignKey(d => d.BlockedUserId)
                .HasConstraintName("FK_BlockedUser");

            entity.HasOne(d => d.BlockingUser).WithMany(p => p.BlockBlockingUsers)
                .HasForeignKey(d => d.BlockingUserId)
                .HasConstraintName("FK_BlockingUser");
        });

        modelBuilder.Entity<Board>(entity =>
        {
            entity.Property(e => e.BoardDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BoardName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.User).WithMany(p => p.Boards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Boards_Users");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Comment1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Comment");

            entity.HasOne(d => d.Pin).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Pins");
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.FollowerId).HasName("PK__Follower__E859401977B56088");

            entity.HasOne(d => d.UserFollower).WithMany(p => p.FollowerUserFollowers)
                .HasForeignKey(d => d.UserFollowerId)
                .HasConstraintName("FK_UserFollower");

            entity.HasOne(d => d.UserFollowing).WithMany(p => p.FollowerUserFollowings)
                .HasForeignKey(d => d.UserFollowingId)
                .HasConstraintName("FK_UserFollowing");
        });

        modelBuilder.Entity<Pin>(entity =>
        {
            entity.HasKey(e => e.PinId).HasName("PK_Images");

            entity.Property(e => e.AltText)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Link)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Pins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pins_Users");
        });

        modelBuilder.Entity<PinBoardAssociation>(entity =>
        {
            entity.HasKey(e => e.PinBoardId);

            entity.ToTable("PinBoardAssociation");

            entity.HasOne(d => d.Board).WithMany(p => p.PinBoardAssociations)
                .HasForeignKey(d => d.BoardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PinBoardAssociation_Boards");

            entity.HasOne(d => d.Pin).WithMany(p => p.PinBoardAssociations)
                .HasForeignKey(d => d.PinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PinBoardAssociation_Pins");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Information)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WebSite)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Profiles_Users");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.Property(e => e.PinId).HasColumnName("PinID");

            entity.HasOne(d => d.Pin).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.PinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reactions_Pins");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.Property(e => e.Reply1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Reply");

            entity.HasOne(d => d.Comment).WithMany(p => p.Replies)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Replies_Comments");
        });

        modelBuilder.Entity<Saved>(entity =>
        {
            entity.ToTable("Saved");

            entity.HasOne(d => d.Pin).WithMany(p => p.Saveds)
                .HasForeignKey(d => d.PinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Saved_Pins");

            entity.HasOne(d => d.User).WithMany(p => p.Saveds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Saved_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pass).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
