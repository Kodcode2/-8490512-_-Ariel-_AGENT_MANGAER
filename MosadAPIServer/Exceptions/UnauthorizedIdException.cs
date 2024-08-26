namespace MosadAPIServer.Exceptions
{
    public class UnauthorizedIdException : Exception
    {
        public UnauthorizedIdException(string? message = "Unauthorized id"):base(message) { }
    }
}
