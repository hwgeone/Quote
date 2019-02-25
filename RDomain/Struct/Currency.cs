using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDomain.Struct
{
    public class Currency
    {
        public static string USD = "USD";
        public static string CNY = "CNY";
        public static string EUR = "EUR";
        public static string TWD = "NTD";
        public static string GBP = "GBP";
        public static string CAD = "CAD";
        public static string JPY = "JPY";
        public static string KRW = "KRW";
        public static string SGD = "KRW";

        public static List<string> GetCurrencys(bool exceptUSD)
        {
            List<string> curs = new List<string>();
            curs.Add(CNY);
            curs.Add(EUR);
            curs.Add(TWD);
            curs.Add(GBP);
            curs.Add(CAD);
            curs.Add(JPY);
            curs.Add(KRW);
            curs.Add(SGD);

            if (!exceptUSD)
            {
                curs.Add(USD);
            }
            return curs;
        }
    }
}
