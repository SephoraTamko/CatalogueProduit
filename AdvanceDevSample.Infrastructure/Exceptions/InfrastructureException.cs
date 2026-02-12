using System;
using System.Collections.Generic;
using System.Text;

namespace AdvanceDevSample.Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException() { }
        public InfrastructureException(string message) : base(message) { }
        public InfrastructureException(string message, Exception innerException) : base(message, innerException) { }
    }
}
