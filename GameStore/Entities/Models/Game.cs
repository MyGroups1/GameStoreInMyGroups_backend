namespace Entities.Models;

public class Game:BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }

    
   // public ICollection<Photo>? Photos { get; set; }
    // public ICollection<Genre>? Genres { get; set; }
    public User? User { get; set; }

    
}