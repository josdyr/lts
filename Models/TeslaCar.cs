using System.ComponentModel.DataAnnotations;

public class TeslaCar
{
    public int Id { get; set; }
    [Required]
    public string? Model { get; set; }
    [Required]
    public string? SerialNumber { get; set; }
    [Required]
    public string? Location { get; set; }
}
