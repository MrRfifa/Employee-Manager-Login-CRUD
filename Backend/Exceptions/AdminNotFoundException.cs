using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Exceptions
{
    public class AdminNotFoundException : Exception
    {
        public AdminNotFoundException() { }
        public AdminNotFoundException(string message) : base(message) { }
        public AdminNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}