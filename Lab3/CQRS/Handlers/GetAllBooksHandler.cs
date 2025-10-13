using BookApi.CQRS.Queries;
using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.CQRS.Handlers;

public class GetAllBooksHandler
{
    private readonly AppDbContext _context;

    public GetAllBooksHandler(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Book>> Handle(GetAllBooksQuery query)
        => await _context.Books.AsNoTracking().ToListAsync();
}