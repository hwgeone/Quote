using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Exceptions
{
    abstract public class ApplicationException : Exception
    {
        /// <summary>Parameterized constructor.	
        /// </summary>	
        public ApplicationException(string errorMessage) : base(errorMessage) { }
    }	
}
