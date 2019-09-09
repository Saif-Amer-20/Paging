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

        [HttpGet("{page}/{limit}")]
        public ActionResult GetProjects(int? page, int? limit)
        {
            int total;
            var records = GetJsonData(page, limit, out total);

            var result = Json(new { records, total });

            return result;
        }

        public List<Project> GetJsonData(int? page, int? limit, out int total)
        {
            var records = _context.Projects.Select(p => p).AsQueryable();

            total = records.Count();

      
            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }


            return records.ToList();
        }
    }
}