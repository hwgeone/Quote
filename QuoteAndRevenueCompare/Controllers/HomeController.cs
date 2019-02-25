using Infrastructure.HttpHelper;
using Newtonsoft.Json;
using QuoteAndRevenueCompare.Utils;
using RApplication;
using RApplication.Binder;
using RApplication.ReportingModel;
using RDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuoteAndRevenueCompare.Controllers
{
    public class ReturnObject
    {
        public string flag { get; set; }
        public string html { get; set; }
    }

    public class HomeController : Controller
    {
        public ReportApp app { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult ViewModal()
        {
            return PartialView();
        }

        [HttpGet]
        public string GetList(int limit, int offset, string filter, string sort, string order)
        {
            var query = app.GetProjects();
            if (!string.IsNullOrEmpty(filter))
            {
                try
                {
                    ProjectView filterObj = JsonConvert.DeserializeObject<ProjectView>(filter);
                    if (ReferenceEquals(filterObj, null))
                    {

                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(filterObj.AccountName))
                        {
                            query = query.Where(s => s.AccountName.ToLower().Contains(filterObj.AccountName.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.OpportunityOwner))
                        {
                            query = query.Where(s => s.OpportunityOwner.ToLower().Contains(filterObj.OpportunityOwner.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.Territory))
                        {
                            query = query.Where(s => s.Territory.ToLower().Contains(filterObj.Territory.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.QuoteNumber))
                        {
                            query = query.Where(s => s.QuoteNumber.ToLower().Contains(filterObj.QuoteNumber.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.StudySite))
                        {
                            query = query.Where(s => s.StudySite.ToLower().Contains(filterObj.StudySite.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.ProjectLine))
                        {
                            query = query.Where(s => s.ProjectLine.ToLower().Contains(filterObj.ProjectLine.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.KickOffDate))
                        {
                            query = query.Where(s => s.KickOffDate.ToLower().Contains(filterObj.KickOffDate.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.Status))
                        {
                            query = query.Where(s => s.Status.ToLower().Contains(filterObj.Status.ToLower()));
                        }
                        if (!string.IsNullOrWhiteSpace(filterObj.ProjectClosedDate))
                        {
                            query = query.Where(s => s.ProjectClosedDate.ToLower().Contains(filterObj.ProjectClosedDate.ToLower()));
                        }
                    }
                }
                catch
                {
                    query = null;
                }
            }

            List<Project> projects;
            int total;
            if (query != null)
            {
                try
                {
                    total = query.Count();
                    //排序
                    if (string.IsNullOrEmpty(sort))
                    {
                        //query = query.OrderBy("EntryDate", "desc");
                    }
                    else
                    {
                        //query = query.OrderBy(sort, order);
                    }
                    //分页
                    query = query.Skip((offset / limit) * limit).Take(limit);

                    projects = query.ToList();
                }
                catch (Exception ex)
                {
                    total = 0;
                    projects = null;
                }
            }
            else
            {
                total = 0;
                projects = null;
            }

            List<ProjectView> views = ReportingBinder.GetProjectViews(projects);

            var model = new
            {
                total = total,
                rows = views,
            };

            return JsonUtils.Instance.Serialize(model);
        }

        [HttpGet]
        public string GetDetailTableHtml(Guid projectId)
        {
            List<Order> orders = app.GetOrders(projectId).ToList();
            List<Income> incomes = app.GetIncomes(projectId).ToList();
            DataTableHtmlGenerator<Order> orderGenerator = new DataTableHtmlGenerator<Order>(orders);
            DataTableHtmlGenerator<Income> incomeGenerator = new DataTableHtmlGenerator<Income>(incomes);

            List<ReturnObject> obj = new List<ReturnObject>();
            obj.Add(new ReturnObject { flag = "order",html = orderGenerator.GetHtml()});
            obj.Add(new ReturnObject { flag = "income", html = incomeGenerator.GetHtml() });
            return JsonUtils.Instance.Serialize(obj);
        }

        //public FileResult DownloadReporting(string year, string month)
        //{

        //}
    }
}
