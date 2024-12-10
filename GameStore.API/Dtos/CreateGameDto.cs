using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GameStore.API.Dtos;

public record class CreateGameDto(
   [Required][StringLength(50)] string Name,
   int GenreId,
   [Required][Range(1, 150)] decimal Price,
   DateOnly ReleaseDate);

