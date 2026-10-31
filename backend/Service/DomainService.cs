using App.Model;
using App.Repository;
using App.Common;
using static App.Common.Input;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace App.Service;

public class DomainService(IDomainRepository repo)
{

  public async Task<object> Boards() => await repo.Query<Board>().OrderBy(b => b.CreatedAt).ToListAsync();
  public async Task<object> CreateBoard(JsonElement j) { await using var tx = await repo.Begin(); var b = new Board { Name = Text(j, "name", 100, true), Description = Text(j, "description", 500) }; repo.Add(b); int pos = 0; foreach (var name in new[] { "Yapılacak", "Devam ediyor", "Tamamlandı" }) repo.Add(new Stage { BoardId = b.Id, Name = name, Position = pos++ }); await repo.Save(); await tx.CommitAsync(); return b; }
  public async Task<object> Board(Guid id) { var b = await repo.Find<Board>(id); var stages = await repo.Query<Stage>().Where(s => s.BoardId == id).OrderBy(s => s.Position).ToListAsync(); var ids = stages.Select(s => s.Id).ToList(); var cards = await repo.Query<Card>().Where(c => ids.Contains(c.StageId)).OrderBy(c => c.CreatedAt).ToListAsync(); return new { board = b, stages = stages.Select(s => new { stage = s, cards = cards.Where(c => c.StageId == s.Id) }) }; }
  public async Task<object> Stage(Guid id, JsonElement j) { await using var tx = await repo.Begin(); await repo.Lock<Board>(id); var s = new Stage { BoardId = id, Name = Text(j, "name", 60, true), Position = await repo.Query<Stage>().CountAsync(s => s.BoardId == id) }; repo.Add(s); await repo.Save(); await tx.CommitAsync(); return s; }
  public async Task<object> CreateCard(JsonElement j) { var c = new Card { StageId = Id(j, "stageId") }; await repo.Find<Stage>(c.StageId); Apply(c, j); repo.Add(c); await repo.Save(); return c; }
  public async Task<object> UpdateCard(Guid id, JsonElement j) { await using var tx = await repo.Begin(); var c = await repo.Lock<Card>(id); Apply(c, j); await repo.Save(); await tx.CommitAsync(); return c; }
  private void Apply(Card c, JsonElement j) { c.Title = Text(j, "title", 160, true); c.Description = Text(j, "description", 2000); c.Assignee = Text(j, "assignee", 80); c.Label = Text(j, "label", 30); c.Priority = Choice(j, "priority", "LOW", "NORMAL", "HIGH"); c.DueDate = j.TryGetProperty("dueDate", out var d) && d.ValueKind != JsonValueKind.Null && d.GetString() != "" ? Date(j, "dueDate") : null; }
  public async Task<object> Move(Guid id, JsonElement j) { await using var tx = await repo.Begin(); var c = await repo.Lock<Card>(id); var from = await repo.Find<Stage>(c.StageId); var to = await repo.Find<Stage>(Id(j, "stageId")); if (from.BoardId != to.BoardId) throw new ApiException(409, "Kart başka panoya taşınamaz."); c.StageId = to.Id; await repo.Save(); await tx.CommitAsync(); return c; }
  public async Task Delete(Guid id) { await using var tx = await repo.Begin(); repo.Remove(await repo.Lock<Card>(id)); await repo.Save(); await tx.CommitAsync(); }

}
