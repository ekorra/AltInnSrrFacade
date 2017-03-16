using System;
using System.Collections.Generic;
using System.Text;

namespace AltInnSrr
{
    public class AltInnSrrException: Exception
    {
        public AltInnSrrException()
        {
        }

        public AltInnSrrException(string message): base(message)
        {
        }

        public AltInnSrrException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
