namespace Entities.Models;

public class Photo:BaseEntity
{
    public byte[]? Bytes { get; set; }
    public string? Description { get; set; }
    public string? FileExtension { get; set; }
    public decimal Size { get; set; }
    // public int GameId { get; set; }
   
   // public Game? Game { get; set; }
}