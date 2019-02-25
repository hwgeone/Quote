using RDomain.Entity;
using RDomain.Rules;
using RDomain.Rules.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Flat
{
    public class Total:IFormat
    {
        public float TotalBooking { get; set; }
        public float TotalCost { get; set; }
        public float Difference { get; set; }
        public float Ratio { get; set; }

        public void Format()
        {
            new FormatObjectFactory<Total>(this).FormatObject();
        }
    }
}
