namespace App.Model;

public class Board
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
}
