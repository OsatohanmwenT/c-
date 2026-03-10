using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class UpdateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][Range(1, int.MaxValue)] int GenreId,
    [Required][Range(0, 100)] decimal Price,
    [Required] DateOnly ReleaseDate
);