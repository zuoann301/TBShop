using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException()
        {
        }
        public ServiceException(string message)
            : base(message)
        {
        }
    }
}
