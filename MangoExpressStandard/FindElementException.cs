using System;
namespace MangoExpressStandard
{
    public class FindElementException : Exception
    {
        public FindElementException()
        {
            throw new NotImplementedException();
        }

        public FindElementException(string message) : base(message)
        {
        }

        public FindElementException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
