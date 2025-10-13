using BookApi.CQRS.Commands;
using BookApi.CQRS.Handlers;
using BookApi.CQRS.Queries;
using BookApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=books.db"));

builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<GetBookByIdHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapPost("/books", async (CreateBookCommand cmd, CreateBookHandler handler) =>
{
    var book = await handler.Handle(cmd);
    return Results.Created($"/books/{book.Id}", book);
});

app.MapGet("/books/{id:int}", async (int id, GetBookByIdHandler handler) =>
{
    var book = await handler.Handle(new GetBookByIdQuery(id));
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

app.MapGet("/books", async (GetAllBooksHandler handler) =>
{
    var books = await handler.Handle(new GetAllBooksQuery());
    return Results.Ok(books);
});

app.MapDelete("/books/{id:int}", async (int id, DeleteBookHandler handler) =>
{
    var success = await handler.Handle(new DeleteBookCommand(id));
    return success ? Results.NoContent() : Results.NotFound();
});

app.Run();