using BookApi.CQRS.Commands;
using BookApi.CQRS.Requests;
using BookApi.Data;
using BookApi.Models;
using BookApi.Common.Exceptions;
using BookApi.Common.Validators;
using Microsoft.EntityFrameworkCore;

namespace BookApi.CQRS.Handlers;

public class CreateBookHandler(AppDbContext context, ILogger<CreateBookHandler> logger)
{
    public async Task<IResult> Handle(CreateBookProfileRequest request)
    {
        logger.LogInformation("Creating new book with Title: {Title}, Author: {Author}, and ISBN: {ISBN}", 
            request.Title, request.Author, request.ISBN);
        
        var validator = new CreateBookProfileRequestValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation failed for book creation: {Errors}", validationResult.Errors);
            return Results.BadRequest(validationResult.Errors);
        }
        
        var existingBook = await context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ISBN == request.ISBN);

        if (existingBook != null)
        {
            logger.LogWarning("Duplicate ISBN detected: {ISBN}", request.ISBN);
            throw new DuplicateIsbnException(request.ISBN);
        }

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            Category = request.Category,
            Price = request.Price,
            PublishedDate = request.PublishedDate,
            CoverImageUrl = request.CoverImageUrl,
            StockQuantity = request.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };

        context.Books.Add(book);
        await context.SaveChangesAsync();
        
        logger.LogInformation("Book created successfully with ID: {BookId}, Title: {Title}", book.Id, book.Title);

        return Results.Created($"/books/{book.Id}", book);
    }
}