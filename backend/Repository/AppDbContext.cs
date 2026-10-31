using Microsoft.EntityFrameworkCore;
using App.Model;
namespace App.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Board> Boards => Set<Board>();
  public DbSet<Stage> Stages => Set<Stage>();
  public DbSet<Card> Cards => Set<Card>();
  protected override void OnModelCreating(ModelBuilder b)
  {
    b.Entity<Board>(e => { e.ToTable("boards"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); e.Property(x => x.CreatedAt).HasColumnName("created_at"); e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired(); e.Property(x => x.Description).HasColumnName("description").HasMaxLength(500).IsRequired(); });
    b.Entity<Stage>(e => { e.ToTable("stages"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); e.Property(x => x.CreatedAt).HasColumnName("created_at"); e.Property(x => x.BoardId).HasColumnName("board_id"); e.Property(x => x.Name).HasColumnName("name").HasMaxLength(60).IsRequired(); e.Property(x => x.Position).HasColumnName("position"); });
    b.Entity<Card>(e => { e.ToTable("cards"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); e.Property(x => x.CreatedAt).HasColumnName("created_at"); e.Property(x => x.StageId).HasColumnName("stage_id"); e.Property(x => x.Title).HasColumnName("title").HasMaxLength(160).IsRequired(); e.Property(x => x.Description).HasColumnName("description").HasMaxLength(2000).IsRequired(); e.Property(x => x.Assignee).HasColumnName("assignee").HasMaxLength(80).IsRequired(); e.Property(x => x.Label).HasColumnName("label").HasMaxLength(30).IsRequired(); e.Property(x => x.Priority).HasColumnName("priority").HasMaxLength(10).IsRequired(); e.Property(x => x.DueDate).HasColumnName("due_date"); });
    b.Entity<Stage>().HasOne<Board>().WithMany().HasForeignKey(x => x.BoardId).OnDelete(DeleteBehavior.Restrict);
    b.Entity<Card>().HasOne<Stage>().WithMany().HasForeignKey(x => x.StageId).OnDelete(DeleteBehavior.Restrict);
  }
}
