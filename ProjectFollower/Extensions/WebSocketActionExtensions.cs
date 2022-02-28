using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using static ProjectFollower.Utility.ProjectConstant;

namespace ProjectFollower.Extensions
{
    public class WebSocketActionExtensions
    {
        protected IHubContext<HomeHub> _context;
        private readonly IUnitOfWork _uow;
        public WebSocketActionExtensions(IHubContext<HomeHub> context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }

        public int Sequence = 0;
        public int Delayeds = 0;
        public async Task ListProjects_WebSocket(Claim Claims)
        {
            /*
            IEnumerable<Projects> Projects;
            List<Projects> _projects = new List<Projects>();
            List<ProjectListVM> ProjectListVMs = new List<ProjectListVM>();
            
            Projects = _uow.Project.GetAll(i => i.Archived == false, includeProperties: "Customers");


            var FilteredProject = Projects.OrderBy(d => Convert.ToDateTime(d.EndingDate));
            foreach (var item in FilteredProject)
            {
                item.SequanceDate = Sequence++;
                if (DateTime.Now.Date > Convert.ToDateTime(item.EndingDate))
                {
                    item.ProjectSequence = 1;
                    item.IsDelayed = true;
                    if (item.Status < 3)
                        Delayeds++;
                }
                else
                    item.ProjectSequence = 2;
                _projects.Add(item);
            }
            var _ProjectListVM = new ProjectListVM()
            {
                Projects = _projects,
                DelayedProjects = Delayeds
            };

            HomeHub Hub = new HomeHub(_context);
            await Hub.SendDataTable(_ProjectListVM);*/
            HomeHub Hub = new HomeHub(_context);
            await Hub.SendDataTable(null);

            //return _ProjectListVM;
        }
        public async Task SchedulerQuery_WebSocket(Claim Claims, string id)
        {
            HomeHub Hub = new HomeHub(_context);
            await Hub.TriggerSchedulerChange(id);
        }
    }
}
