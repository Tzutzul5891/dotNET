using BookApi.Data;
using BookApi.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace BookApi.CQRS.Handlers;

public class GetAllBooksHandler(AppDbContext context, ILogger<GetAllBooksHandler> logger)
{
    public async Task<IResult> Handle()
    {
        logger.LogInformation("Fetching all books");
        
        var books = await context.Books.AsNoTracking().ToListAsync();
        
        logger.LogInformation("Found {BookCount} books", books.Count);
        return Results.Ok(books);
    }
}