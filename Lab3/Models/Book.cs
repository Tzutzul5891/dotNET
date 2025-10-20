namespace BookApi.Models;

public class Book
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public required string Title { get; set; }
    
    public required string Author { get; set; }

    public required string ISBN { get; set; }
    
    public BookCategory Category { get; set; }
    
    public decimal Price { get; set; }
    
    public DateTime PublishedDate { get; set; }

    public string? CoverImageUrl { get; set; }
    
    public int StockQuantity { get; set; } = 0;
    
    public bool IsAvailable {
        get => StockQuantity > 0;
        set { /* intentionally left blank - availability derived from StockQuantity */ }
    }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}