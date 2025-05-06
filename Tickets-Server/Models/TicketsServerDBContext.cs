using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

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

    public virtual DbSet<PurchaseRequest> PurchaseRequests { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=Tickets_Server;User ID=AdminLogin;Password=Ran1234;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.FeedBackId).HasName("PK__FeedBack__E2CB3B87673E3BC4");
        });

        modelBuilder.Entity<PurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Purchase__712CC60708A3F78C");

            entity.Property(e => e.TicketId).ValueGeneratedNever();

            entity.HasOne(d => d.BuyerEmailNavigation).WithMany(p => p.PurchaseRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseR__Buyer__34C8D9D1");

            entity.HasOne(d => d.Ticket).WithOne(p => p.PurchaseRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseR__Ticke__33D4B598");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.RankId).HasName("PK__Ranks__B37AF876758CC278");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__Teams__123AE7999F5689E8");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC6071D4387F0");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.TicketAwayTeams).HasConstraintName("FK__Tickets__AwayTea__30F848ED");

            entity.HasOne(d => d.Team).WithMany(p => p.TicketTeams).HasConstraintName("FK__Tickets__TeamId__2F10007B");

            entity.HasOne(d => d.UserEmailNavigation).WithMany(p => p.Tickets).HasConstraintName("FK__Tickets__UserEma__300424B4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Users__A9D10535D186A84A");

            entity.HasOne(d => d.FavoriteTeam).WithMany(p => p.Users).HasConstraintName("FK__Users__FavoriteT__2C3393D0");

            entity.HasOne(d => d.FeedBack).WithMany(p => p.Users).HasConstraintName("FK__Users__FeedBackI__2B3F6F97");

            entity.HasOne(d => d.Rank).WithMany(p => p.Users).HasConstraintName("FK__Users__RankId__2A4B4B5E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
