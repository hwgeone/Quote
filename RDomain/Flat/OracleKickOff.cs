using RDomain.Entity;
using RDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Flat
{
    public class OracleKickOff:IFormat
    {
        public string QuoteNumber { get; set; }
        public string KickOffDate { get; set; }

        public void Format()
        {
            new FormatObjectFactory<OracleKickOff>(this).FormatObject();
        }
    }
}
