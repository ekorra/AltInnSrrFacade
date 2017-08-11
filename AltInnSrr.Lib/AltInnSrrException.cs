using System;
using System.Collections.Generic;
using AltInnSrr.Lib.Connected_Services.AltInnSrrService;

namespace AltInnSrr.Lib
{
    public class AltInnSrrException: Exception
    {
        public  IList<OperationResult?> AltInnFaultResult { get; private set; }
        public AltInnSrrException(IList<OperationResult?> result)
        {
            AltInnFaultResult = result;
        }

        public AltInnSrrException(string message): base(message)
        {
        }
        public AltInnSrrException(string message, List<OperationResult?> result) : this(message)
        {
            AltInnFaultResult = result;
        }

        public AltInnSrrException(string message, Exception innerException, IList<OperationResult> result): base(message, innerException)
        {
        }
    }
}
