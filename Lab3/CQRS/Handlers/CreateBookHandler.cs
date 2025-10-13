using BookApi.CQRS.Commands;
using BookApi.Data;
using BookApi.Models;

namespace BookApi.CQRS.Handlers;

public class CreateBookHandler
{
    private readonly AppDbContext _context;

    public CreateBookHandler(AppDbContext context) => _context = context;

    public async Task<Book> Handle(CreateBookCommand command)
    {
        var book = new Book { Title = command.Title, Author = command.Author, Year = command.Year };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }
}