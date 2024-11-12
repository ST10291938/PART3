using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PART3.Data;
using PART3.Models;

namespace PART3.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lecturers.ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,lecture_ID,lecturer_Name,lecturer_Surname,lecturer_Email,lecturer_Contact,Program,Module_Code,Hours_Worked,Hourly_Rate,Date_Of_Session,UploadedFileName,Status")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,lecture_ID,lecturer_Name,lecturer_Surname,lecturer_Email,lecturer_Contact,Program,Module_Code,Hours_Worked,Hourly_Rate,Date_Of_Session,UploadedFileName,Status")] Lecturer lecturer)
        {
            if (id != lecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.Id))
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
            return View(lecturer);
        }


        // GET: Lecturers/Approve/5
        public async Task<IActionResult> Approve(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return RedirectToAction(nameof(Index));
            }

            lecturer.Status = "Approved";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Lecturers/Reject/5
        public async Task<IActionResult> Reject(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return RedirectToAction(nameof(Index));
            }

            lecturer.Status = "Rejected"; // Update status to Rejected
            await _context.SaveChangesAsync(); // Save changes to the database
            return RedirectToAction(nameof(Index));
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }
    }
}
