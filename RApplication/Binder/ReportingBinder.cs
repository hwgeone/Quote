using RApplication.ReportingModel;
using RDomain.Entity;
using RDomain.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication.Binder
{
    /// <summary>
    /// AutoMapper可取代之
    /// </summary>
    public class ReportingBinder
    {
        public static ProjectView GetProjectView(Project project)
        {
            ProjectView projectView = new ProjectView
            {
                Id = project.Id,
                AccountName = project.AccountName,
                OpportunityOwner = project.OpportunityOwner,
                Territory = project.Territory,
                QuoteNumber = project.QuoteNumber.Trim(),
                StudySite = project.StudySite,
                ProjectLine = project.ProjectLine,
                TotalBooking = project.TotalBooking,
                KickOffDate = project.KickOffDate,
                Status = project.Status,
                ProjectClosedDate = project.ProjectClosedDate,
                USDCurrency = Currency.USD,
                FinalCost = project.FinalCost,
                different = project.different
            };

            return projectView;
        }

        public static List<ProjectView> GetProjectViews(List<Project> projects)
        {
            List<ProjectView> views = new List<ProjectView>();

            foreach (var project in projects)
            {
                ProjectView view = GetProjectView(project);
                views.Add(view);
            }

            return views;
        }
     }
}
