﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PayrollApp.Controllers
{
    public class PayrollsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
