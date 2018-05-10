using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarfetchExercise.Data;
using FarfetchExercise.Models;
using FarfetchExercise.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FarfetchExercise.Controllers
{
    public class PointsController : Controller
    {
        private readonly IPointsRepository _pointsRepository;

        public PointsController(IPointsRepository pointsRepository)
        {
            _pointsRepository = pointsRepository;
        }

        // GET: Points
        public async Task<IActionResult> Index()
        {
            IEnumerable<Point> points = await _pointsRepository.GetPoints();
            return View(points);
        }

        // GET: Points/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _pointsRepository.GetPointByID(id.Value);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // GET: Points/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Points/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Point point)
        {
            if (ModelState.IsValid)
            {
                _pointsRepository.InsertPoint(point);
                await _pointsRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(point);
        }

        // GET: Points/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _pointsRepository.GetPointByID(id.Value);
            if (point == null)
            {
                return NotFound();
            }
            return View(point);
        }

        // POST: Points/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Point point)
        {
            if (id != point.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _pointsRepository.UpdatePoint(point);
                    await _pointsRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var pointExists = await PointExists(point.Id);
                    if (!pointExists)
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
            return View(point);
        }

        // GET: Points/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _pointsRepository.GetPointByID(id.Value);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // POST: Points/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _pointsRepository.DeletePoint(id);
            await _pointsRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PointExists(int id)
        {
            return await _pointsRepository.GetPointByID(id) != null;
        }
    }
}
