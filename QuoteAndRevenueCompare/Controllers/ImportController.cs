using Infrastructure.HttpHelper;
using QuoteAndRevenueCompare.Models;
using RApplication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuoteAndRevenueCompare.Controllers
{
    public class ImportController : Controller
    {
        private static readonly log4net.ILog mylogrecoder = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImportApp app { get; set; }

        private async Task GetMethodDynamic(Stream stream,string fileName)
        {
            if (fileName.Contains("SalesForce"))
            {
                await app.ImportForSalesForce(stream, fileName);
            }
            else if (fileName.Contains("OracleKickOff"))
            {
                await app.ImportForOracleKickOff(stream, fileName);
            }
            else if(fileName.Contains("OracleCost"))
            {
                await app.ImportForOracleCost(stream, fileName);
            }
            else if (fileName.Contains("ExchangeRate"))
            {
                await app.ImportForExchangeRate(stream, fileName);
            }
            else
            {
                throw new Exception("导入的文件名字必须是下载的模板的名字");
            }
        }

        public async Task<ActionResult> Import()
        {
            Response res = new Response();
            var file = Request.Files["uploadfile"];
            Stream stream = file.InputStream;
            try {
                await GetMethodDynamic(stream,file.FileName);
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                mylogrecoder.Error(ex.Message);
            }
            return Json(res);
        }
    }
}
