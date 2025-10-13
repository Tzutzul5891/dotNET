using BookApi.Models;

namespace BookApi.CQRS.Commands;

public record CreateBookCommand(string Title, string Author, int Year);