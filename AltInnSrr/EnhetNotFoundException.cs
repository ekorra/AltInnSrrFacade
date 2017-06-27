using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AltInnSrr.Lib
{
    public class EnhetNotFoundException : Exception
    {
        public EnhetNotFoundException()
        {
        }

        public EnhetNotFoundException(string message) : base(message)
        {
        }

        public EnhetNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
