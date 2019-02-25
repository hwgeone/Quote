using RDomain.Enum;
using RDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Entity
{
    public class Order:Entity,IFormat
    {
        public Guid ProjectId { get; set; }
        public string QuoteNumber { get; set; }
        public string Type { get; set; }
        public string AmountCurrency { get; set; }
        public float Amount { get; set; }
        public string BookingCloseDate { get; set; }

        public void Check()
        {
            string trimType = Type.Trim().ToLower();
            if (trimType != OrderType.NewProject && trimType != OrderType.Amendment)
            {
                throw new Exception("Type is not valid, must be 'New project' or 'amendment'. ");
            }
        }

        public void Format()
        {
            new FormatObjectFactory<Order>(this).FormatObject();
        }
    }
}
