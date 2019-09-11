using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PagingTask.Data;
using PagingTask.DBContext;
using Project = PagingTask.Data.Project;

namespace PagingTask.Controllers
{
    [Route("paging/[action]")]
    public class PagingController : Controller
    {

        private readonly ApplicationDbContext _context;


        public PagingController(ApplicationDbContext context) 
        {
            _context = context;
         
        }

        [HttpGet("{page}")]
        public ActionResult GetProjects(int? page)
        {
            var records = GetJsonData(page, out int total);

            var result = Json(new { records, total });

            return result;
        }

        public List<Project> GetJsonData(int? page,  out int total)
        {
            var records = _context.Projects.OrderBy(p=>p.Name).AsQueryable();

            total = records.Count();

      
            if (page.HasValue )
            {
                int start = (page.Value - 1) * 10;
                records = records.Skip(start).Take(10);
            }


            return records.ToList();
        }
    }
}