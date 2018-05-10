using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarfetchExercise.Data;
using FarfetchExercise.Models;
using FarfetchExercise.Repository;
using FarfetchExercise.Repository.Interfaces;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace FarfetchExercise.Controllers
{
    public class RoutesController : Controller
    {
        private IRoutesRepository _routesRepository;
        private IPointsRepository _pointsRepository;

        public RoutesController(IRoutesRepository routesRepository, IPointsRepository pointsRepository)
        {
            _routesRepository = routesRepository;
            _pointsRepository = pointsRepository;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            IEnumerable<Route> routes = await _routesRepository.GetRoutes();
            return View(routes);
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _routesRepository.GetRouteByID(id.Value);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Routes/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            List<Point> points = await _pointsRepository.GetPoints();
            ViewData["OriginId"] = new SelectList(points, "Id", "Name");
            ViewData["DestinationId"] = new SelectList(points, "Id", "Name");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Time,Cost,OriginId,DestinationId")] Route route)
        {
            if (ModelState.IsValid)
            {
                _routesRepository.InsertRoute(route);
                await _routesRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            List<Point> points = await _pointsRepository.GetPoints();
            ViewData["OriginId"] = new SelectList(points, "Id", "Name", route.OriginId);
            ViewData["DestinationId"] = new SelectList(points, "Id", "Name", route.DestinationId);
            return View(route);
        }

        // GET: Routes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _routesRepository.GetRouteByID(id.Value);
            if (route == null)
            {
                return NotFound();
            }

            List<Point> points = await _pointsRepository.GetPoints();
            ViewData["OriginId"] = new SelectList(points, "Id", "Name", route.OriginId);
            ViewData["DestinationId"] = new SelectList(points, "Id", "Name", route.DestinationId);
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Time,Cost,OriginId,DestinationId")] Route route)
        {
            if (id != route.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _routesRepository.UpdateRoute(route);
                    await _routesRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var routeExists = await RouteExists(route.Id);
                    if (!routeExists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            List<Point> points = await _pointsRepository.GetPoints();
            ViewData["OriginId"] = new SelectList(points, "Id", "Name", route.OriginId);
            ViewData["DestinationId"] = new SelectList(points, "Id", "Name", route.DestinationId);
            return View(route);
        }

        // GET: Routes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _routesRepository.GetRouteByID(id.Value);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _routesRepository.DeleteRoute(id);
            await _routesRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            _routesRepository.Dispose();
            _pointsRepository.Dispose();
            base.Dispose(disposing);
        }

        private async Task<bool> RouteExists(int id)
        {
            return await _routesRepository.GetRouteByID(id) != null;
        }
    }
}
