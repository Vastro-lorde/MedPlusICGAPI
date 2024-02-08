namespace MedPlusICGAPI.Helpers
{
    public class NoDataFoundException : Exception
    {
        public NoDataFoundException() : base() { }
        public NoDataFoundException(string message) : base(message) { }
        public NoDataFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

}
