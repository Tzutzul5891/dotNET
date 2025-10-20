using BookApi.CQRS.Commands;
using BookApi.Data;
using BookApi.Common.Exceptions;

namespace BookApi.CQRS.Handlers;

public class DeleteBookHandler
{
    private readonly AppDbContext _context;

    public DeleteBookHandler(AppDbContext context) => _context = context;

    public async Task Handle(DeleteBookCommand command)
    {
        var book = await _context.Books.FindAsync(command.Id);
        
        if (book == null)
        {
            throw new BookNotFoundException(command.Id);
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}