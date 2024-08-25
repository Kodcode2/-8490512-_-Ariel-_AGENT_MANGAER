namespace MosadAPIServer.Exceptions
{
    public class InvalidDirectionException : Exception
    {
        public InvalidDirectionException(string? message = "Invalid direction") : base(message) { }
    
    }
}
