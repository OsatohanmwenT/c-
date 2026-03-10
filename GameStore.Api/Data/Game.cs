namespace GameStore.Api.Data;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; } = null!;
    public Genre? Genre { get; set; } = null!;

    public int GenreId { get; set; }
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
}