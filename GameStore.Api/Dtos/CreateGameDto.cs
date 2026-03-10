using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class CreateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][Range(1, 50)] int GenreId,
    [Required][Range(0, 100)] decimal Price,
    [Required] DateOnly ReleaseDate
);