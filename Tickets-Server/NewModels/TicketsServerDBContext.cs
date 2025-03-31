using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.NewModels;

public partial class TicketsServerDBContext : DbContext
{
    public TicketsServerDBContext()
    {
    }

    public TicketsServerDBContext(DbContextOptions<TicketsServerDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=Tickets_Server;User ID=AdminLogin;Password=Ran1234;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.FeedBackId).HasName("PK__FeedBack__E2CB3B87B945EF67");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.RankId).HasName("PK__Ranks__B37AF876FB920526");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__Teams__123AE799CC1E9EAB");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC607BCF7C7E7");

            entity.HasOne(d => d.Team).WithMany(p => p.Tickets).HasConstraintName("FK__Tickets__TeamId__2E1BDC42");

            entity.HasOne(d => d.UserEmailNavigation).WithMany(p => p.Tickets).HasConstraintName("FK__Tickets__UserEma__2F10007B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Users__A9D10535180BB6B0");

            entity.HasOne(d => d.FeedBack).WithMany(p => p.Users).HasConstraintName("FK__Users__FeedBackI__29572725");

            entity.HasOne(d => d.Rank).WithMany(p => p.Users).HasConstraintName("FK__Users__RankId__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
