using System;

namespace TestGamma
{
    class IncorrectInputException : ApplicationException
    {
        public IncorrectInputException() { }
        public IncorrectInputException(string message) : base(message)
        { }
    }
}