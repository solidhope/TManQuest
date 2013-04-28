using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDEngine
{
    public class MissingServiceException : Exception
    {
        public MissingServiceException(string message) : base(message) { }
    }
}
