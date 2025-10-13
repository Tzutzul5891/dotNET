using BookApi.CQRS.Queries;
using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.CQRS.Handlers;

public class GetBookByIdHandler
{
    private readonly AppDbContext _context;

    public GetBookByIdHandler(AppDbContext context) => _context = context;

    public async Task<Book?> Handle(GetBookByIdQuery query)
        => await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == query.Id);
}