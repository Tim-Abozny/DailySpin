﻿using DailySpin.Logic.Services;
using DailySpin.ViewModel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DailySpin.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PeriodicHostedService _periodicService;

        public HomeController(ILogger<HomeController> logger,
            PeriodicHostedService periodicService)
        {
            _periodicService = periodicService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}