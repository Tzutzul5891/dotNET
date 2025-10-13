using BookApi.CQRS.Commands;
using BookApi.Data;

namespace BookApi.CQRS.Handlers;

public class DeleteBookHandler
{
    private readonly AppDbContext _context;

    public DeleteBookHandler(AppDbContext context) => _context = context;

    public async Task<bool> Handle(DeleteBookCommand command)
    {
        var book = await _context.Books.FindAsync(command.Id);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}