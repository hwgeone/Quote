using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Flat
{
    public class FlatExchangeRate
    {
        public string Currency { get; set; }
        /// <summary>
        /// yyyy/MM
        /// </summary>
        public string YearMonthDate { get; set; }
        public float Rate { get; set; }
    }
}
