namespace MosadAPIServer.Exceptions
{
    public class InvalidAction : Exception
    {
        public InvalidAction(string? message):base(message) { }
    }
}
