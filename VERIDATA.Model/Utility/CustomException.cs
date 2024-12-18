namespace VERIDATA.Model.utility
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException() : base()
        {
        }

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}