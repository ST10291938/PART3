using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PART3.Data;
using PART3.Models;

namespace PART3.Controllers
{
    public class LecturersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public LecturersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Lecturers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lecturers.ToListAsync());
        }

        // GET: Lecturers/Details/5
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

        // GET: Lecturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,lecture_ID,lecturer_Name,lecturer_Surname,lecturer_Email,lecturer_Contact,Program,Module_Code,Hours_Worked,Hourly_Rate,Date_Of_Session,UploadedFileName,Status")] Lecturer lecturer, IFormFile? uploadedFileName)
        {
            if (ModelState.IsValid)
            {
                // Handle the file upload if a file was submitted
                if (uploadedFileName != null)
                {
                    // Set the directory path for file storage
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Ensure the directory exists
                    Directory.CreateDirectory(uploadsFolder);

                    // Generate a unique file name to prevent conflicts
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadedFileName.FileName);

                    // Get the full path to save the file
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Copy the uploaded file to the target location
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFileName.CopyToAsync(fileStream);
                    }

                    // Set the file path in the model
                    lecturer.UploadedFileName = uniqueFileName;
                }




                lecturer.Status = "Pending";


                _context.Add(lecturer);
                await _context.SaveChangesAsync();
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
                TempData["Error"] = "Lecturer not found.";
                return RedirectToAction(nameof(Index));
            }

            lecturer.Status = "Approved";
            await _context.SaveChangesAsync();
            TempData["Message"] = $"{lecturer.lecturer_Name} {lecturer.lecturer_Surname}'s claim has been approved.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Lecturers/Reject/5
        public async Task<IActionResult> Reject(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                TempData["Error"] = "Lecturer not found.";
                return RedirectToAction(nameof(Index));
            }

            lecturer.Status = "Rejected"; // Update status to Rejected
            await _context.SaveChangesAsync(); // Save changes to the database
            TempData["Message"] = $"{lecturer.lecturer_Name} {lecturer.lecturer_Surname}'s claim has been rejected.";
            return RedirectToAction(nameof(Index));
        }


        // GET: Lecturers/Edit/5
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

        // POST: Lecturers/Edit/5
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

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }

        /*
        // GET: Lecturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer != null)
            {
                _context.Lecturers.Remove(lecturer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturers.Any(e => e.Id == id);
        }
    }
        */
    }
}
