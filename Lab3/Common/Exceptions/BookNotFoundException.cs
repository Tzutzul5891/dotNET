namespace BookApi.Common.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException(Guid bookId) 
        : base($"Book with ID '{bookId}' was not found.")
    {
        BookId = bookId;
    }

    public Guid BookId { get; }
}