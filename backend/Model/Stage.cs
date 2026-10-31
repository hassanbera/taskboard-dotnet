namespace App.Model;

public class Stage
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public Guid BoardId { get; set; } = Guid.Empty;
  public string Name { get; set; } = "";
  public int Position { get; set; } = 0;
}
