namespace GameStore.Api.Dto;

public record class GameSummaryDto(
int Id,
string Name,
string Genre,
decimal Price,
DateOnly ReleaseDate);

