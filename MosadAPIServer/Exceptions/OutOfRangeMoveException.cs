namespace MosadAPIServer.Exceptions
{
    public class OutOfRangeMoveException : Exception
    {
        public OutOfRangeMoveException(string? message = "Cannot move out of bounds") : base(message) { }
    
    }
}
