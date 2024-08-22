using GameStore.Api.Contracts;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
	const string GetGameEndpointName = "GetGame";

	private static readonly List<GameContract> games = [
		new (1, "Elden Ring", "RPG", 54.99M, new DateOnly(2022, 02, 25)),
	new (2, "Dredge", "Fishing/Story", 34.99M, new DateOnly(2023, 03, 30))
	];

	public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("games").WithParameterValidation();
		// GET /games
		group.MapGet("/", () => games);

		// GET /games/{id}
		group.MapGet("/{id}", (int id) =>
		{
			var game = games.Find(game => game.Id == id);

			return game is null ? Results.NotFound() : Results.Ok(game);

		}).WithName(GetGameEndpointName);

		// POST /games
		group.MapPost("/", (CreateGameContract newGame) =>
		{
			GameContract game = new(
				games.Count + 1,
				newGame.Name,
				newGame.Genre,
				newGame.Price,
				newGame.ReleaseDate
			);

			games.Add(game);

			return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
		});

		// PUT /games/{id}
		group.MapPut("/{id}", (int id, UpdateGameContract updatedGame) =>
		{
			var index = games.FindIndex(game => game.Id == id);

			if (index == -1)
			{
				return Results.NotFound();
			}

			games[index] = new GameContract(
				id,
				updatedGame.Name,
				updatedGame.Genre,
				updatedGame.Price,
				updatedGame.ReleaseDate
			);

			return Results.NoContent();
		});

		// DELETE /games/{id}
		group.MapDelete("/{id}", (int id) =>
		{
			games.RemoveAll(game => game.Id == id);

			return Results.NoContent();
		});

		return group;
	}
}
