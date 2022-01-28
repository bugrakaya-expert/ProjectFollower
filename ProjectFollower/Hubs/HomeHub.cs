using Abp.Dependency;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFollower.Hubs
{
    public class HomeHub : Hub, ITransientDependency
    {/*
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);//WORKING!!
        }*/
        /*
        private readonly ILogger<HomeHub> _logger;
        private readonly IUnitOfWork _uow;
        public HomeHub(IUnitOfWork uow, ILogger<HomeHub> logger)
        {
            _logger = logger;
            _uow = uow;
        }*/
        protected IHubContext<HomeHub> _context;
        public IAbpSession AbpSession { get; set; }
        public HomeHub(IHubContext<HomeHub> context)
        {
            AbpSession = NullAbpSession.Instance;
            _context = context;
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task SendDataTable(ProjectListVM ProjectList)
        {
            //await Clients.All("SendData", ProjectList);
            await _context.Clients.All.SendAsync("SendData", ProjectList);
            //await Clients.All.SendAsync("SendData", ProjectList);
        }
    }
}
