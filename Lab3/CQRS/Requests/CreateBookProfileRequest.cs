// filepath: /Users/thinslicesacademy17/RiderProjects/dotNET/Lab3/CQRS/Requests/CreateBookProfileRequest.cs
using BookApi.Models;

namespace BookApi.CQRS.Requests;

public class CreateBookProfileRequest
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public BookCategory Category { get; set; }
    public decimal Price { get; set; }
    public DateTime PublishedDate { get; set; }
    public string? CoverImageUrl { get; set; }
    public int StockQuantity { get; set; } = 1;
}

