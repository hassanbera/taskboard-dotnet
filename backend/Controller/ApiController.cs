using Microsoft.AspNetCore.Mvc;
using App.Service;
using App.Repository;
using System.Text.Json;
namespace App.Controller;

[ApiController]
[Route("api")]
public class ApiController(DomainService service, IDomainRepository repo) : ControllerBase
{
  [HttpGet("health")] public async Task<object> Health() { if (!await repo.Healthy()) throw new Exception("Database unavailable"); return new { data = new { status = "ok" } }; }
  [HttpGet("boards")] public async Task<object> Action0() { return new { data = await service.Boards() }; }
  [HttpPost("boards")] public async Task<object> Action1([FromBody] JsonElement j) { return new { data = await service.CreateBoard(j) }; }
  [HttpGet("boards/{id}")] public async Task<object> Action2(Guid id) { return new { data = await service.Board(id) }; }
  [HttpPost("boards/{id}/stages")] public async Task<object> Action3(Guid id, [FromBody] JsonElement j) { return new { data = await service.Stage(id, j) }; }
  [HttpPost("cards")] public async Task<object> Action4([FromBody] JsonElement j) { return new { data = await service.CreateCard(j) }; }
  [HttpPut("cards/{id}")] public async Task<object> Action5(Guid id, [FromBody] JsonElement j) { return new { data = await service.UpdateCard(id, j) }; }
  [HttpPatch("cards/{id}/move")] public async Task<object> Action6(Guid id, [FromBody] JsonElement j) { return new { data = await service.Move(id, j) }; }
  [HttpDelete("cards/{id}")] public async Task<object> Action7(Guid id) { await service.Delete(id); return new { data = true }; }
}
