using System;

namespace NTS.Common.Utils
{
    public class NTSException: Exception
    {
        public NTSException()
        { }

        public NTSException(string message)
            : base(message)
        { }

        public NTSException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
