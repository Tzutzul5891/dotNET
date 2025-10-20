using BookApi.Models;

namespace BookApi.CQRS.Commands;

public record CreateBookCommand(
    string Title,
    string Author,
    string ISBN,
    BookCategory Category,
    decimal Price,
    DateTime PublishedDate,
    string? CoverImageUrl,
    int StockQuantity
);