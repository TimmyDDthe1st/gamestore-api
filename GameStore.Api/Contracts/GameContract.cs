namespace GameStore.Api.Contracts;

public record class GameContract(
	int Id,
	string Name,
	string Genre,
	decimal Price,
	DateOnly ReleaseDate
);
