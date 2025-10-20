using BookApi.CQRS.Commands;
using BookApi.CQRS.Handlers;
using BookApi.CQRS.Queries;
using BookApi.CQRS.Requests;
using BookApi.Data;
using BookApi.Common.Middleware;
using BookApi.Common.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=books.db"));

builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<GetBookByIdHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookProfileRequestValidator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapPost("/books", async (CreateBookProfileRequest req, CreateBookHandler handler) =>
{
    return await handler.Handle(req);
});

app.MapGet("/books/{id:guid}", async (Guid id, GetBookByIdHandler handler) =>
{
    var book = await handler.Handle(new GetBookByIdQuery(id));
    return Results.Ok(book);
});

app.MapGet("/books", async (GetAllBooksHandler handler) =>
{
    return await handler.Handle();
});

app.MapDelete("/books/{id:guid}", async (Guid id, DeleteBookHandler handler) =>
{
    await handler.Handle(new DeleteBookCommand(id));
    return Results.NoContent();
});

app.Run();
