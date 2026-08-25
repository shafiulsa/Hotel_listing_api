using System.ComponentModel.DataAnnotations;

namespace hotellisting.api.DTOs.Country;

public class UpdateCountryDto:CreateCountryDto
{
    [Required]
    public int Id { get; set; }
}
