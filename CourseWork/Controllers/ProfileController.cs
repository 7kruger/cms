using CourseWork.DAL;
using CourseWork.Domain.Entities;
using CourseWork.Domain.ViewModels;
using CourseWork.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
