using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarfetchExercise.Data;
using FarfetchExercise.Models;
using FarfetchExercise.Services;
using FarfetchExercise.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarfetchExercise.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PathsController : Controller
    {
        private readonly IPathsService _service;

        public PathsController(IPathsService service)
        {
            _service = service;
        }

        // GET: Routes
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string startNode, string endNode)
        {
            if(startNode == null || endNode == null)
            {
                return NotFound();
            }
            List<Path> paths = await _service.FindPathsBetweenNodes("A", "B");
            return Ok(paths);
        }

    }
}