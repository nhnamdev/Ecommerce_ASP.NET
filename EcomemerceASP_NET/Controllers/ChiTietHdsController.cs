using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcomemerceASP_NET.Data;

namespace EcomemerceASP_NET.Controllers
{
    public class ChiTietHdsController : Controller
    {
        private readonly EcommerceContext _context;

        public ChiTietHdsController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: ChiTietHds
        public async Task<IActionResult> Index()
        {
            var ecommerceContext = _context.ChiTietHds.Include(c => c.MaHdNavigation).Include(c => c.MaHhNavigation);
            return View(await ecommerceContext.ToListAsync());
        }

        // GET: ChiTietHds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietHd = await _context.ChiTietHds
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaHhNavigation)
                .FirstOrDefaultAsync(m => m.MaCt == id);
            if (chiTietHd == null)
            {
                return NotFound();
            }

            return View(chiTietHd);
        }

        // GET: ChiTietHds/Create
        public IActionResult Create()
        {
            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd");
            ViewData["MaHh"] = new SelectList(_context.HangHoas, "MaHh", "MaHh");
            return View();
        }

        // POST: ChiTietHds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCt,MaHd,MaHh,DonGia,SoLuong,GiamGia")] ChiTietHd chiTietHd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietHd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd", chiTietHd.MaHd);
            ViewData["MaHh"] = new SelectList(_context.HangHoas, "MaHh", "MaHh", chiTietHd.MaHh);
            return View(chiTietHd);
        }

        // GET: ChiTietHds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietHd = await _context.ChiTietHds.FindAsync(id);
            if (chiTietHd == null)
            {
                return NotFound();
            }
            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd", chiTietHd.MaHd);
            ViewData["MaHh"] = new SelectList(_context.HangHoas, "MaHh", "MaHh", chiTietHd.MaHh);
            return View(chiTietHd);
        }

        // POST: ChiTietHds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCt,MaHd,MaHh,DonGia,SoLuong,GiamGia")] ChiTietHd chiTietHd)
        {
            if (id != chiTietHd.MaCt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietHd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietHdExists(chiTietHd.MaCt))
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
            ViewData["MaHd"] = new SelectList(_context.HoaDons, "MaHd", "MaHd", chiTietHd.MaHd);
            ViewData["MaHh"] = new SelectList(_context.HangHoas, "MaHh", "MaHh", chiTietHd.MaHh);
            return View(chiTietHd);
        }

        // GET: ChiTietHds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietHd = await _context.ChiTietHds
                .Include(c => c.MaHdNavigation)
                .Include(c => c.MaHhNavigation)
                .FirstOrDefaultAsync(m => m.MaCt == id);
            if (chiTietHd == null)
            {
                return NotFound();
            }

            return View(chiTietHd);
        }

        // POST: ChiTietHds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTietHd = await _context.ChiTietHds.FindAsync(id);
            if (chiTietHd != null)
            {
                _context.ChiTietHds.Remove(chiTietHd);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietHdExists(int id)
        {
            return _context.ChiTietHds.Any(e => e.MaCt == id);
        }
    }
}
