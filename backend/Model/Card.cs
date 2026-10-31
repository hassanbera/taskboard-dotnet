namespace App.Model;

public class Card
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public Guid StageId { get; set; } = Guid.Empty;
  public string Title { get; set; } = "";
  public string Description { get; set; } = "";
  public string Assignee { get; set; } = "";
  public string Label { get; set; } = "Genel";
  public string Priority { get; set; } = "NORMAL";
  public DateOnly? DueDate { get; set; } = null;
}
