using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Models;

namespace Noested.Controllers
{
    public class ChecklistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecklistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checklists
        public async Task<IActionResult> Index()
        {
            return _context.Checklist != null ?
                        View(await _context.Checklist.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Checklist'  is null.");
        }

        // GET: Checklists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Checklist == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .FirstOrDefaultAsync(m => m.ChecklistId == id);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        // GET: Checklists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Checklists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChecklistId,OrderId,ServiceProcedure,ApprovedBy,PreparedBy,MechBrakes,MechDrumBearing,MechStoragePTO,MechWire,MechChainTensioner,MechPinionBearing,MechClutch,MechSprocketWedges,HydCylinder,HydHydraulicBlock,HydTankOil,HydGearboxOil,HydBrakeCylinder,ElCableNetwork,ElRadio,ElButtonBox,TensionCheckBar,TestWinch,TestTraction,TestBrakes,RepairComment,MechSignature,DateCompleted")] Checklist checklist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checklist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checklist);
        }

        // GET: Checklists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Checklist == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist.FindAsync(id);
            if (checklist == null)
            {
                return NotFound();
            }
            return View(checklist);
        }

        // POST: Checklists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChecklistId,OrderId,ServiceProcedure,ApprovedBy,PreparedBy,MechBrakes,MechDrumBearing,MechStoragePTO,MechWire,MechChainTensioner,MechPinionBearing,MechClutch,MechSprocketWedges,HydCylinder,HydHydraulicBlock,HydTankOil,HydGearboxOil,HydBrakeCylinder,ElCableNetwork,ElRadio,ElButtonBox,TensionCheckBar,TestWinch,TestTraction,TestBrakes,RepairComment,MechSignature,DateCompleted")] Checklist checklist)
        {
            if (id != checklist.ChecklistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checklist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistExists(checklist.ChecklistId))
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
            return View(checklist);
        }

        // GET: Checklists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Checklist == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .FirstOrDefaultAsync(m => m.ChecklistId == id);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        // POST: Checklists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Checklist == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Checklist'  is null.");
            }
            var checklist = await _context.Checklist.FindAsync(id);
            if (checklist != null)
            {
                _context.Checklist.Remove(checklist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChecklistExists(int id)
        {
            return (_context.Checklist?.Any(e => e.ChecklistId == id)).GetValueOrDefault();
        }
    }
}